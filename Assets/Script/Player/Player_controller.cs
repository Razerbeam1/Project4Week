using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("Control Settings")]
    [SerializeField] private float speed; // ความเร็วการเคลื่อนที่
    [SerializeField] private float jumpForce; // แรงกระโดด
    [SerializeField] private Transform absorbPoint; // จุดที่ใช้ดูดวัตถุ
    [SerializeField] private float absorbRange; // ระยะการดูดวัตถุ
    [SerializeField] private GameObject[] absorbableObjects; // วัตถุที่สามารถดูดได้
    [SerializeField] private float sizeIncreaseFactor; // ปริมาณการขยายตัวของ Player ต่อวัตถุหนึ่งชิ้น
    [SerializeField] private Image[] absorbCountImages; // Array ของรูปภาพ UI
    [SerializeField] private int maxAbsorbableObjects; // จำนวนวัตถุสูงสุดที่ดูดได้

    private Rigidbody2D rb;
    private bool isGrounded = false;
    private List<GameObject> absorbedObjects = new List<GameObject>(); // เก็บวัตถุที่ถูกดูด

    private float initialScale; // ขนาดเริ่มต้นของ Player
    private int jumpCount = 0; // ตัวนับจำนวนการกระโดด
    [SerializeField] private int maxJumpCount; // จำนวนการกระโดดสูงสุด
    
    
    [Header("Weight Settings")]
    [SerializeField] private float weightIncreasePerObject; // น้ำหนักที่เพิ่มต่อวัตถุที่ดูด
    private float weight; // น้ำหนักของ Player
    [SerializeField] private float baseWeight; // น้ำหนักเริ่มต้นของ Player

    
    [Header("Hp Settings")]
    [SerializeField] public Image[] hpImages; // อ้างอิงถึงจุดรูปภาพ HP ทั้งหมด
    [SerializeField] int hp; // จำนวน HP เริ่มต้นของผู้เล่น

    [Header("Mana Settings")]
    [SerializeField] private Image[] manaImages; // รูปภาพของ Bar มานา
    [SerializeField] private float manaRechargeTime; // เวลาที่ใช้ในการรีเซ็ตมานาจุดละ 1 จุด
    [SerializeField] private float manaConsumptionTime; // เวลาที่ใช้ในการลดมานาจุดละ 1 จุด
    private int currentMana; // มานาปัจจุบัน
    private float manaTimer; // ตัวจับเวลาสำหรับการลดมานา
    private bool isAbsorbing; // ตรวจสอบว่าผู้เล่นกำลังกดดูดวัตถุอยู่หรือไม่
    
    [Header("Trash Can Settings")]
    [SerializeField] private TrashCan trashCan; // อ้างอิงถึง TrashCan ที่แยกออกมา
    
    [Header("Lose Game Settings")]
    [SerializeField] private GameObject loseCanvas; // Canvas Lose
    [SerializeField] private GameObject gamePlayCanvas; // Canvas Game Play
    
    
    private float initialColliderRadius; //รัศมีเริ่มต้นของ Collider
    
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        initialScale = transform.localScale.x; // เก็บขนาดเริ่มต้นไว้
        
        weight = baseWeight; // ตั้งค่าน้ำหนักเริ่มต้น
        UpdateAbsorbCountUI(); // อัปเดต UI เริ่มต้น
        UpdateHPUI(); // อัปเดต UI ตอนเริ่มเกม
        
        currentMana = manaImages.Length; // เริ่มต้นมานาเต็ม
        UpdateManaUI();
        
        // หาตำแหน่งเช็คพอยต์ล่าสุดที่ผ่าน
        // เมื่อเริ่มเกม ให้เช็คจุดที่เช็คพอยต์ล่าสุดและย้ายผู้เล่นไปยังจุดนั้น
        for (int i = Checkpoint.checkpointReached.Length - 1; i >= 0; i--)
        {
            if (Checkpoint.checkpointReached[i]) 
            {
                // รีเซ็ตตำแหน่งผู้เล่นไปยังจุดที่บันทึก
                transform.position = Checkpoint.checkpointPositions[i];
                break;
            }
        }
        
        CircleCollider2D playerCollider = GetComponent<CircleCollider2D>();
        if (playerCollider != null)
        {
            initialColliderRadius = playerCollider.radius;
        }
    }

    private void Update()
    {
        HandleMovement();
        HandleJump();
        //HandleAbsorption();
        HandleReleaseObjects();
        HandleAbsorptionWithMana();
        RechargeMana();
    }
    
    #region <HandleMovement> // ควบคุมการเคลื่อนที่
    private void HandleMovement()
    {
        float moveInput = 0f;

        if (Input.GetKey(KeyCode.A))
        {
            moveInput = -1f;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            moveInput = 1f;
        }

        // ปรับความเร็วตามน้ำหนัก
        float adjustedSpeed = speed / (1 + weight / 100);
        rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);

        // ปรับการหันหน้าตัวละคร
        if (moveInput != 0)
        {
            // หมุนตัว Player เพื่อแสดงการกลิ้ง
            float rotationSpeed = 360f; // ปรับค่าความเร็วการหมุน (องศาต่อวินาที)
            transform.Rotate(0, 0, -moveInput * rotationSpeed * Time.deltaTime);

            // กำหนดให้ Player หันซ้ายหรือขวา
            transform.localScale = new Vector3(moveInput > 0 ? Mathf.Abs(transform.localScale.x) : -Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }
    #endregion 
    
    #region <HandleJump> //ควบคุมการกระโดด
    private void HandleJump() //ควบคุมการกระโดด
    {
        // ตรวจสอบว่าอยู่บนพื้นและจำนวนการกระโดดยังไม่เกินจำนวนสูงสุด
        if (Input.GetKeyDown(KeyCode.W) && (isGrounded || jumpCount < maxJumpCount))
        {
            // ปรับแรงกระโดดตามน้ำหนัก
            float adjustedJumpForce = jumpForce / (1 + weight / 100);
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            if (isGrounded)
            {
                jumpCount = 1; // รีเซ็ตและเริ่มต้นกระโดดที่ 1 ครั้งเมื่อสัมผัสพื้น
            }
            else
            {
                jumpCount++; // เพิ่มจำนวนการกระโดดถ้ากระโดดอยู่ในอากาศ
            }
        }
    }
    #endregion
    

    /*private void HandleAbsorption()
    {
        if (Input.GetMouseButtonDown(0)) // คลิกซ้ายเพื่อดูดวัตถุ
        {
            if (absorbedObjects.Count >= maxAbsorbableObjects) // ตรวจสอบว่าเกินขีดจำกัดหรือไม่
            {
                Debug.Log("ไม่สามารถดูดวัตถุได้ เก็บครบแล้ว!");
                return;
            }

            Collider2D[] hits = Physics2D.OverlapCircleAll(absorbPoint.position, absorbRange);

            foreach (Collider2D hit in hits)
            {
                if (IsAbsorbable(hit.gameObject))
                {
                    GameObject obj = hit.gameObject;

                    // เพิ่มขนาด Player แบบเท่ากันทุกด้าน
                    float newScale = transform.localScale.x + sizeIncreaseFactor;
                    transform.localScale = new Vector3(newScale, newScale, newScale);

                    // ลบวัตถุออกจาก Scene และเพิ่มเข้าไปใน List
                    absorbedObjects.Add(obj);
                    obj.SetActive(false); // ซ่อนวัตถุแทนการลบ
                    UpdateAbsorbCountUI(); // อัปเดต UI
                    Debug.Log("ดูดวัตถุ: " + obj.name);
                    return;
                }
            }

            Debug.Log("ไม่พบวัตถุในระยะดูด");
        }
    }*/

    /*private void HandleReleaseObjects()
    {
        if (Input.GetKeyDown(KeyCode.E) && absorbedObjects.Count > 0) // กด E เพื่อปล่อยวัตถุ
        {
            // ดึงวัตถุจาก List และแสดงในตำแหน่ง absorbPoint
            GameObject obj = absorbedObjects[absorbedObjects.Count - 1];
            absorbedObjects.RemoveAt(absorbedObjects.Count - 1);

            obj.transform.position = absorbPoint.position;
            obj.SetActive(true); // แสดงวัตถุใหม่
            
            // ตรวจสอบว่าทิ้งลงในถังขยะหรือไม่
            if (trashCan != null)
            {
                // ใช้ Collider2D ตรวจสอบว่าไปทิ้งในพื้นที่ของ TrashCan หรือไม่
                Collider2D objCollider = obj.GetComponent<Collider2D>();
                if (trashCan.GetComponent<Collider2D>().IsTouching(objCollider))
                {
                    trashCan.AddObjectToTrash(); // เพิ่มจำนวนวัตถุในถังขยะ
                }
            }

            // ลดขนาด Player แบบเท่ากันทุกด้าน
            float newScale = transform.localScale.x - sizeIncreaseFactor;

            // ตรวจสอบว่าขนาดไม่ต่ำกว่าขนาดเริ่มต้น
            newScale = Mathf.Max(newScale, initialScale);
            transform.localScale = new Vector3(newScale, newScale, newScale);

            UpdateAbsorbCountUI(); // อัปเดต UI
            Debug.Log("ปล่อยวัตถุ: " + obj.name);
        }
    }*/
    
    #region <HandleAbsorptionWithMana> //กดปุ่มซ้ายค้างเพื่อดูดวัตถุพร้อมกับการใช้มานาในการดูด
    private void HandleAbsorptionWithMana() //กดปุ่มซ้ายค้างเพื่อดูดวัตถุพร้อมกับการใช้มานาในการดูด
    {
        if (Input.GetMouseButton(0)) // กดคลิกซ้ายค้าง
        {
            isAbsorbing = true;
            manaTimer += Time.deltaTime;

            if (manaTimer >= manaConsumptionTime && currentMana > 0)
            {
                // ลดมานา
                currentMana--;
                manaTimer = 0f; // รีเซ็ตตัวจับเวลา
                UpdateManaUI();

                if (currentMana > 0)
                {
                    AbsorbObjects(); // ทำการดูดวัตถุ
                }
                else
                {
                    Debug.Log("มานาหมด! ไม่สามารถดูดวัตถุได้");
                }
            }
        }
        else
        {
            isAbsorbing = false;
        }
    }
    #endregion
    
    #region <HandleReleaseObjects> //ปล่อยวัตถุที่ผู้เล่นดูดมา
    private void HandleReleaseObjects() //ปล่อยวัตถุที่ผู้เล่นดูดมา
    {
        if (Input.GetKeyDown(KeyCode.E) && absorbedObjects.Count > 0) // กด E เพื่อปล่อยวัตถุ
        {
            // ดึงวัตถุจาก List และปล่อยออก
            GameObject obj = absorbedObjects[absorbedObjects.Count - 1];
            absorbedObjects.RemoveAt(absorbedObjects.Count - 1);

            obj.transform.parent = null; // แยก Obj ออกจาก Player
            obj.SetActive(true); // เปิดใช้งานวัตถุ

            // เพิ่มฟิสิกส์เพื่อโยนวัตถุออกไป
            Rigidbody2D objRb = obj.GetComponent<Rigidbody2D>();
            if (objRb != null)
            {
                objRb.simulated = true; // เปิดการคำนวณฟิสิกส์
                Vector2 throwDirection = (obj.transform.position - transform.position).normalized; // ทิศทางการปล่อย
                objRb.velocity = throwDirection * 5f; // ปรับ 5f เพื่อควบคุมความเร็ว
            }

            // อัปเดตขนาดของ CircleCollider2D
            UpdatePlayerCollider();

            UpdateAbsorbCountUI(); // อัปเดต UI
            Debug.Log("โยนวัตถุออก: " + obj.name);
        }
    }
    #endregion
    
    /*private void AbsorbObjects()
    {
        // เมื่อกดคลิกซ้ายค้างและมานาพร้อม
        if (currentMana > 0)
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(absorbPoint.position, absorbRange);

            foreach (Collider2D hit in hits)
            {
                if (IsAbsorbable(hit.gameObject))
                {
                    GameObject obj = hit.gameObject;

                    // เพิ่มขนาด Player แบบเท่ากันทุกด้าน
                    float newScale = transform.localScale.x + sizeIncreaseFactor;
                    transform.localScale = new Vector3(newScale, newScale, newScale);
                    
                    weight += weightIncreasePerObject; // เพิ่มน้ำหนักตามวัตถุที่ดูด
                    
                    // ลบวัตถุออกจาก Scene และเพิ่มเข้าไปใน List
                    absorbedObjects.Add(obj);
                    obj.SetActive(false); // ซ่อนวัตถุแทนการลบ
                    UpdateAbsorbCountUI(); // อัปเดต UI
                    Debug.Log("ดูดวัตถุ: " + obj.name);
                    return;
                }
            }

            Debug.Log("ไม่พบวัตถุในระยะดูด");
        }
    }*/
    
    #region <AbsorbObjects> //ดูดวัตถุที่อยู่ในระยะที่กำหนด
    private void AbsorbObjects() //ดูดวัตถุที่อยู่ในระยะที่กำหนด
    {
        // ตรวจจับวัตถุที่อยู่รอบ Player ในระยะ absorbRange
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, absorbRange);

        foreach (Collider2D hit in hits)
        {
            if (IsAbsorbable(hit.gameObject))
            {
                GameObject obj = hit.gameObject;

                // ตั้งค่าให้ Obj ติดกับ Player โดยตรง
                obj.transform.parent = this.transform; // ทำให้ Obj เป็นลูกของ Player
                float angle = Random.Range(0, 360); // มุมสุ่มรอบตัว Player
                float radius = Random.Range(0.5f, 1.0f); // ระยะห่างจาก Player
                obj.transform.localPosition = new Vector3(
                    Mathf.Cos(angle * Mathf.Deg2Rad) * radius,
                    Mathf.Sin(angle * Mathf.Deg2Rad) * radius,
                    0
                );

                // ปิดการคำนวณฟิสิกส์ของ Obj
                Rigidbody2D objRb = obj.GetComponent<Rigidbody2D>();
                if (objRb != null)
                {
                    objRb.simulated = false; // ปิดการคำนวณฟิสิกส์
                }

                // ปิดการชนกันระหว่าง Player และ Obj
                Collider2D objCollider = obj.GetComponent<Collider2D>();
                Collider2D playerCollider = GetComponent<Collider2D>();
                if (objCollider != null && playerCollider != null)
                {
                    Physics2D.IgnoreCollision(objCollider, playerCollider, true); // ปิดการชนกันกับ Player
                }

                // เพิ่ม Obj เข้า List ของวัตถุที่ดูดได้
                absorbedObjects.Add(obj);

                // อัปเดตขนาด Collider ของ Player
                UpdatePlayerCollider();

                Debug.Log("ดูดวัตถุ: " + obj.name);
                UpdateAbsorbCountUI(); // อัปเดต UI

                return;
            }
        }

        Debug.Log("ไม่พบวัตถุในระยะดูด");
    }
    #endregion

    #region <UpdatePlayerCollider> //อัปเดตขนาดของ CircleCollider2D
    private void UpdatePlayerCollider() //อัปเดตขนาดของ CircleCollider2D
    {
        // เช็คว่ามี Collider ชนิด CircleCollider2D หรือไม่
        CircleCollider2D playerCollider = GetComponent<CircleCollider2D>();
        if (playerCollider != null)
        {
            // เก็บค่า radius เริ่มต้นไว้
            float initialRadius = 0.5f; // กำหนดค่ารัศมีเริ่มต้นที่ต้องการ

            // ระยะใหม่ให้ครอบคลุมทุก Obj ที่ดูดติด
            float maxDistance = 0f;

            foreach (GameObject obj in absorbedObjects)
            {
                float distance = Vector3.Distance(transform.position, obj.transform.position);
                if (distance > maxDistance)
                {
                    maxDistance = distance;
                }
            }

            // เพิ่มระยะเผื่ออีกเล็กน้อย (0.3f)
            float newRadius = maxDistance + 0.3f;

            // ป้องกันไม่ให้ลดลงต่ำกว่ารัศมีเริ่มต้น
            playerCollider.radius = Mathf.Max(newRadius, initialRadius);

            Debug.Log("ขนาด Collider ใหม่: " + playerCollider.radius);
        }
    }
    #endregion
    
    #region <IsAbsorbable> //ตรวจสอบว่าวัตถุที่ชนสามารถดูดได้หรือไม่ 
    private bool IsAbsorbable(GameObject obj)  //ตรวจสอบว่าวัตถุที่ชนสามารถดูดได้หรือไม่ 
    {
        // ตรวจสอบว่า obj อยู่ใน absorbableObjects หรือไม่
        foreach (GameObject absorbable in absorbableObjects)
        {
            if (obj == absorbable)
            {
                return true;
            }
        }
        return false;
    }
    #endregion

    #region <OnCollisionEnter2D> //มีการชนกับวัตถุชนิด 2D โดยใช้เพื่อตรวจสอบว่าผู้เล่นอยู่บนพื้นหรือไม่ และตรวจจับกับดัก
    private void OnCollisionEnter2D(Collision2D collision) //มีการชนกับวัตถุชนิด 2D โดยใช้เพื่อตรวจสอบว่าผู้เล่นอยู่บนพื้นหรือไม่ และตรวจจับกับดัก
    {
        // เช็คว่าชนพื้นหรือไม่
        if (collision.contacts[0].normal.y > 0.5f)
        {
            isGrounded = true;
            jumpCount = 0; // รีเซ็ตจำนวนการกระโดดเมื่อสัมผัสพื้น
        }
    }
    #endregion
    
    #region <OnCollisionExit2D> //ผู้เล่นไม่อยู่บนพื้นแล้ว
    private void OnCollisionExit2D(Collision2D collision) //ผู้เล่นไม่อยู่บนพื้นแล้ว
    {
        // เมื่อไม่อยู่บนพื้น (อาจจะขึ้นจากพื้นหรืออยู่ในอากาศ)
        isGrounded = false;
    }
    #endregion

    #region <OnDrawGizmos> //Editor แสดงวงกลมระยะการดูดวัตถุ
    private void OnDrawGizmos() //Editor แสดงวงกลมระยะการดูดวัตถุ
    {
        // แสดงระยะการดูดวัตถุใน Scene
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(absorbPoint.position, absorbRange);
    }
    #endregion

    #region <UpdateAbsorbCountUI> //อัปเดต UI ของจำนวนวัตถุที่ผู้เล่นดูดมา
    private void UpdateAbsorbCountUI() //อัปเดต UI ของจำนวนวัตถุที่ผู้เล่นดูดมา
    {
        // แสดง/ซ่อนจุด UI ตามจำนวนวัตถุที่ดูดได้
        for (int i = 0; i < absorbCountImages.Length; i++)
        {
            absorbCountImages[i].enabled = i < absorbedObjects.Count;
        }
    }
    #endregion
    
    #region <OnTriggerEnter2D> //ตรวจจับการชนกับ Trigger
    private void OnTriggerEnter2D(Collider2D other) //ตรวจจับการชนกับ Trigger
    {
        // ตรวจจับการตกจากแมป (ถ้าผู้เล่นตกไปในหลุม)
        if (other.CompareTag("FallTrigger")) // FallTrigger คือตำแหน่งที่ใช้ตรวจสอบการตกจากแมป
        {
            // ให้ Player กลับไปที่จุดเช็คพอยต์ล่าสุด
            for (int i = Checkpoint.checkpointReached.Length - 1; i >= 0; i--)
            {
                if (Checkpoint.checkpointReached[i]) 
                {
                    transform.position = Checkpoint.checkpointPositions[i]; // กลับไปที่เช็คพอยต์ล่าสุด
                    Debug.Log("Player fell. Respawning at checkpoint " + (i + 1));
                    break;
                }
            }
        }
        
        if (other.CompareTag("Trap")) // เมื่อชนกับกับดัก
        {
            // ลด HP และอัปเดต UI
            hp--;
            UpdateHPUI();

            // เช็คว่าถ้า HP หมด (Game Over)
            if (hp <= 0)
            {
                Debug.Log("Game Over!");
                // คุณสามารถเพิ่ม Logic การหยุดเกมหรือแสดงเมนู Game Over ที่นี่
            }
        }
    }
    #endregion
    
    #region <UpdateHPUI> //อัปเดต UI ของ HP ของผู้เล่น
    private void UpdateHPUI() //อัปเดต UI ของ HP ของผู้เล่น
    {
        for (int i = 0; i < hpImages.Length; i++)
        {
            if (i < hp)
            {
                hpImages[i].color = Color.green; // สีปกติ
            }
            else
            {
                hpImages[i].color = new Color(1, 1, 1, 0.5f); // สีจาง (Alpha 0.5)
            }
        }
        
        // ตรวจสอบ HP และเรียก Game Over ถ้า HP หมด
        if (hp <= 0)
        {
            HandleGameOver(); // เรียกฟังก์ชันสำหรับการหยุดเกม
        }
    }
    #endregion
    
    #region <HandleGameOver> //หยุดเกมเมื่อ HP ของผู้เล่นหมด และแสดง UI สำหรับ Game Over
    private void HandleGameOver() //หยุดเกมเมื่อ HP ของผู้เล่นหมด และแสดง UI สำหรับ Game Over
    {
        // หยุดเกม
        Time.timeScale = 0f;

        // แสดง Canvas Lose
        if (loseCanvas != null)
        {
            loseCanvas.SetActive(true);
        }

        // ซ่อน Canvas Game Play
        if (gamePlayCanvas != null)
        {
            gamePlayCanvas.SetActive(false);
        }

        Debug.Log("Game Over! HP หมด.");
    }
    #endregion
    
    #region <RechargeMana> //รีเซ็ตมานา (เพิ่มมานา) เมื่อไม่ได้ทำการดูดวัตถุ
    private void RechargeMana() //รีเซ็ตมานา (เพิ่มมานา) เมื่อไม่ได้ทำการดูดวัตถุ   
    {
        if (!isAbsorbing && currentMana < manaImages.Length) // เมื่อไม่ได้ดูดวัตถุและมานายังไม่เต็ม
        {
            manaTimer += Time.deltaTime;

            if (manaTimer >= manaRechargeTime)
            {
                currentMana++;
                manaTimer = 0f; // รีเซ็ตตัวจับเวลา
                UpdateManaUI();
            }
        }
    }
    #endregion

    #region <UpdateManaUI>
    private void UpdateManaUI() //อัปเดต UI ของมานา
    {
        // แสดง/ซ่อน Bar มานา ตามจำนวนมานาปัจจุบัน
        for (int i = 0; i < manaImages.Length; i++)
        {
            manaImages[i].enabled = i < currentMana;
        }
    }
    #endregion
    
    
}
