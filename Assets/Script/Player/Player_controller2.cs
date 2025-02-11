using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
public class Player_controller2 : MonoBehaviour
{
    [Header("Control Movement")]
    public float moveSpeed = 5f; // ความเร็วในการเคลื่อนที่
    public float groundStickForce = 5f; // แรงยึดติดกับพื้น
    public float edgeSnapDistance = 0.2f; // ระยะห่างที่ใช้ตรวจจับขอบพื้น
    private Rigidbody2D rb2d; // ตัวควบคุม Rigidbody2D

    private bool isOnGround = false; // ตรวจสอบว่าอยู่บนพื้นหรือไม่
    private Vector2 groundNormal; // ทิศทาง Normal ของพื้น
    
    [Header("Control Settings")]
    [SerializeField] private Transform absorbPoint; // จุดที่ใช้ดูดวัตถุ
    [SerializeField] private float absorbRange; // ระยะการดูดวัตถุ
    [SerializeField] private GameObject[] absorbableObjects; // วัตถุที่สามารถดูดได้
    [SerializeField] private float sizeIncreaseFactor; // ปริมาณการขยายตัวของ Player ต่อวัตถุหนึ่งชิ้น //0.6
    [SerializeField] private Image[] absorbCountImages; // Array ของรูปภาพ UI
    [SerializeField] private int maxAbsorbableObjects; // จำนวนวัตถุสูงสุดที่ดูดได้
    
    private bool canAbsorb = true; // ตัวแปรบ่งชี้ว่า Player สามารถดูดวัตถุได้หรือไม่
    private float absorbCooldown = 3f; // เวลาหยุดการดูด (ในวินาที)
    private float currentCooldownTime = 0f; // เวลาปัจจุบันที่หยุดการดูด
    
    private Rigidbody2D rb;
    private bool isGrounded = false;
    private List<GameObject> absorbedObjects = new List<GameObject>(); // เก็บวัตถุที่ถูกดูด

    private float initialScale; // ขนาดเริ่มต้นของ Player
    
    [Header("Weight Settings")]
    [SerializeField] private float weightIncreasePerObject; // น้ำหนักที่เพิ่มต่อวัตถุที่ดูด
    private float weight; // น้ำหนักของ Player
    [SerializeField] private float baseWeight; // น้ำหนักเริ่มต้นของ Player
    
    private float initialColliderRadius; //รัศมีเริ่มต้นของ Collider
    
    // เพิ่มเพื่อให้ Player ร่วงลง
    private bool isFalling = false;
    
    //[Header("--------------------------")]
    // ประกาศตัวแปร playerMass
    [SerializeField] public float playerMass;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        if (rb2d == null)
        {
            Debug.LogError("Rigidbody2D is missing from the Player object! Please add a Rigidbody2D.");
        }
        Rigidbody2D playerRb = GetComponent<Rigidbody2D>();
        if (playerRb != null)
        {
            playerRb.mass = 1f; // กำหนดมวลเริ่มต้นของ Player เป็น 1
        }
        
        // กำหนดมวลเริ่มต้นของ Player
        playerMass = rb2d.mass;
        
        UpdateAbsorbCountUI();

    }

    //public GameObject pausePanel; // Panel ที่ใช้แสดงเมื่อเกมหยุด
    //private int currentIndex = 0;
    
    void Update()
    {
        Movement();
        
        //-------------------
        AbsorbObjects(); // ทำการดูดวัตถุ
        HandleReleaseObjects();
        
        if (!canAbsorb)
        {
            currentCooldownTime -= Time.deltaTime; // ลดเวลาลง
            if (currentCooldownTime <= 0f)
            {
                canAbsorb = true; // ทำให้สามารถดูดได้อีกครั้ง
                Debug.Log("สามารถดูดวัตถุได้อีกครั้ง");
            }
        }
        
        // เพิ่มส่วนนี้เพื่อให้ Player ร่วงลงเมื่อคลิกเมาส์
        if (Input.GetMouseButtonDown(0)) // คลิกเมาส์ซ้าย (Mouse0)
        {
            StopAbsorbingAndFall();
            
        }
        
    }
    

   
    private void StopAbsorbingAndFall()
    { 
        canAbsorb = false;
        rb2d.gravityScale = 1f; // เปิดการโน้มถ่วง เพื่อให้ Player ร่วงลง

        foreach (GameObject obj in absorbedObjects)
        {
            obj.transform.parent = null;
            obj.SetActive(true);

            Rigidbody2D objRb = obj.GetComponent<Rigidbody2D>();
            if (objRb != null)
            {
                objRb.simulated = true;
                objRb.gravityScale = 1f;
            }
        }

        absorbedObjects.Clear(); // ล้างวัตถุที่ดูดติด
        
        // ✅ เพิ่มแรงกระทำให้ Player ร่วงลง
        rb2d.velocity = new Vector2(rb2d.velocity.x, -1f); 

        UpdateAbsorbCountUI(); // อัปเดต UI
        UpdatePlayerCollider(); // 📌 อัปเดตขนาด Collider ใหม่ หลังจากปล่อย Obj

        currentCooldownTime = absorbCooldown;
        Debug.Log("หยุดการดูดและร่วงลงไป 2 วินาที");
    }

    
    private void Movement()
    {
        if (!isOnGround) return; // หยุดทำงานถ้าไม่ได้อยู่บนพื้น

        // รับการเคลื่อนที่จากผู้เล่น
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        // คำนวณทิศทางการเคลื่อนที่
        Vector2 inputDirection = new Vector2(horizontal, vertical);

        // ปรับการเคลื่อนที่ให้สอดคล้องกับพื้น
        Vector2 moveDirection = AdjustMovementToGround(inputDirection) * moveSpeed;

        // ตั้งค่าความเร็วใหม่
        rb2d.velocity = moveDirection;

        // เพิ่มแรงดูดให้ Player ติดกับพื้น
        StickToGround();
        
        // ปรับความเร็วตามน้ำหนัก
        float adjustedSpeed = moveSpeed / (1 + weight / 100);
        
        // ประกาศตัวแปร targetVelocityX, targetVelocityY
        float targetVelocityX = moveDirection.x;
        float targetVelocityY = moveDirection.y;
        
        float newVelocityX = targetVelocityX * adjustedSpeed;
        
        // การหมุนตัวและการหันหน้า
        if (newVelocityX != 0)
        {
            // หมุนตัว Player เพื่อแสดงการกลิ้ง
            float rotationSpeed = 360f; // ปรับค่าความเร็วการหมุน (องศาต่อวินาที)
            transform.Rotate(0, 0, -targetVelocityX * rotationSpeed * Time.deltaTime);

            // กำหนดให้ Player หันซ้ายหรือขวา
            transform.localScale = new Vector3(
                targetVelocityX > 0 ? Mathf.Abs(transform.localScale.x) : -Mathf.Abs(transform.localScale.x),
                transform.localScale.y,
                transform.localScale.z
            );
        }
        
        // ตรวจสอบว่ามีวัตถุติด Player และเล่นเสียงเดิน
        if (absorbedObjects.Count > 0) // ตรวจสอบว่ามีวัตถุติดกับ Player หรือไม่
        {
            AudioManager.Instance.PlayWalkWithObjectSound(); // เรียกใช้เสียงเดินที่มีวัตถุติดกับ Player
        }
    }
    
    // ปรับการเคลื่อนที่ให้ตรงกับแนวพื้น
    Vector2 AdjustMovementToGround(Vector2 inputDirection)
    {
        // โยนทิศทางการเคลื่อนที่ไปยังระนาบที่ขนานกับพื้น
        Vector2 tangent = Vector2.Perpendicular(groundNormal);
        return Vector2.Dot(inputDirection, tangent) * tangent.normalized;
    }

    // เพิ่มแรงดูดให้ Player ติดกับพื้น
    void StickToGround()
    {
        if (!isOnGround) return;

        // ตรวจสอบว่าผู้เล่นไม่ได้อยู่บน TrashObject
        if (isOnGround && !IsOnTrashObject())
        {
            // ใช้แรงในทิศทางของ Normal ของพื้น เพื่อให้ Player ไม่ตก
            Vector2 stickForce = -groundNormal * groundStickForce;
            rb2d.AddForce(stickForce, ForceMode2D.Force);

            // ตรวจสอบขอบพื้นโดยการยิง Raycast
            RaycastHit2D hit = Physics2D.Raycast(transform.position, -groundNormal, edgeSnapDistance, LayerMask.GetMask("Ground"));
            if (hit.collider != null)
            {
                // ถ้า Raycast เจอพื้น ก็จะบังคับให้ Player ติดขอบ
                rb2d.velocity = new Vector2(rb2d.velocity.x, Mathf.Max(rb2d.velocity.y, 0f));
            }
        }
    }

    // ฟังก์ชันเพื่อตรวจสอบว่า Player ชนกับ TrashObject หรือไม่
    bool IsOnTrashObject()
    {
        return GetComponent<Collider2D>().IsTouchingLayers(LayerMask.GetMask("TrashObject"));
    }


    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = true;

            // คำนวณค่าเฉลี่ยของ Normal จากทุกจุดที่ชน
            Vector2 normalSum = Vector2.zero;
            foreach (ContactPoint2D contact in collision.contacts)
            {
                normalSum += contact.normal;
            }
            groundNormal = normalSum.normalized;
        }
        else if (collision.gameObject.CompareTag("TrashObject"))
        {
            // เมื่อชนกับ "TrashObject", ให้ Player สามารถเดินได้โดยไม่ต้องยึดติดกับพื้น
            // ไม่ต้องทำอะไรเพราะว่าไม่ได้ใช้ `groundNormal` หรือ `isOnGround` กับ TrashObject
        }
    }
    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = false;
        }
        else if (collision.gameObject.CompareTag("TrashObject"))
        {
            // ออกจากการชนกับ "TrashObject" ก็สามารถปล่อยให้ Player ยังคงไม่ติดพื้น
            // ไม่ต้องทำอะไรเป็นพิเศษ
        }
    }
    
    #region <HandleReleaseObjects> //ปล่อยวัตถุที่ผู้เล่นดูดมา
    // ฟังก์ชันการปล่อยวัตถุ
    private void HandleReleaseObjects()
    {
        if (Input.GetKeyDown(KeyCode.E) && absorbedObjects.Count > 0) // กด E เพื่อปล่อยวัตถุ
        {
            GameObject obj = absorbedObjects[absorbedObjects.Count - 1];
            absorbedObjects.RemoveAt(absorbedObjects.Count - 1);

            obj.transform.parent = null; // แยก Obj ออกจาก Player
            obj.SetActive(true); // เปิดใช้งานวัตถุ

            // เปิดการคำนวณฟิสิกส์ของ Obj
            Rigidbody2D objRb = obj.GetComponent<Rigidbody2D>();
            if (objRb != null)
            {
                objRb.simulated = true; // เปิดการคำนวณฟิสิกส์
                Vector2 throwDirection = (obj.transform.position - transform.position).normalized; // ทิศทางการปล่อย
                objRb.velocity = throwDirection * 5f; // ปรับ 5f เพื่อควบคุมความเร็ว

                // ลดน้ำหนักที่เพิ่มขึ้นจากการดูดวัตถุ
                weight -= objRb.mass; 
            }

            // อัปเดตขนาดของ CircleCollider2D
            UpdatePlayerCollider();

            // อัปเดต UI
            UpdateAbsorbCountUI(); 

            // ปิดการดูดชั่วคราว (Cooldown)
            canAbsorb = false;
            currentCooldownTime = absorbCooldown; // ตั้งค่าคูลดาวน์เป็นเวลา absorbCooldown (ค่าเริ่มต้นคือ 10 วินาที)
        
            // เปิดการชนของ Collider อีกครั้ง
            Collider2D objCollider = obj.GetComponent<Collider2D>();
            Collider2D playerCollider = GetComponent<Collider2D>();

            if (objCollider != null)
            {
                objCollider.enabled = true; // เปิดใช้งาน Collider ของ Obj
                Debug.Log($"เปิด Collider ของ {obj.name} แล้ว"); // Debug Log
            }

            if (objCollider != null && playerCollider != null)
            {
                Physics2D.IgnoreCollision(objCollider, playerCollider, false); // เปิดให้ชนกันได้อีกครั้ง
                Debug.Log($"เปิดการชนระหว่าง {obj.name} และ Player");
            }

            Debug.Log("โยนวัตถุออก: " + obj.name);
        }
    }

    #endregion
    
    
    
    #region <AbsorbObjects> //ดูดวัตถุที่อยู่ในระยะที่กำหนด
    private void AbsorbObjects()
    {
        if (!canAbsorb) return; // ถ้าไม่สามารถดูดได้ ให้หยุดการทำงาน

        // ตรวจจับวัตถุที่อยู่รอบ Player ในระยะ absorbRange
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, absorbRange);

        foreach (Collider2D hit in hits)
        {
            // เพิ่มการตรวจสอบว่าไม่ใช่วัตถุที่เป็นพื้น
            //if (hit.CompareTag("Ground")) continue; // ถ้าเป็นพื้น ให้ข้ามการดูด
        
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

                // เพิ่มมวลของ Player
                UpdatePlayerMass();

                // อัปเดตขนาด Collider ของ Player
                UpdatePlayerCollider();

                // เพิ่มการควบคุมแรงโน้มถ่วงของ Player ให้เหมาะสม
                Rigidbody2D playerRb = GetComponent<Rigidbody2D>();
                if (playerRb != null)
                {
                    // ถ้าต้องการให้ Player หยุดไหลลง, ปรับแรงโน้มถ่วง
                    playerRb.gravityScale = 0.5f;  // ลดแรงโน้มถ่วงหรือปรับแต่งตามต้องการ
                }
                
                Debug.Log("ดูดวัตถุ: " + obj.name);
                UpdateAbsorbCountUI(); // อัปเดต UI

                return;
            }
        }

         // Debug.Log("ไม่พบวัตถุในระยะดูด");
    }
    #endregion
    
    // ฟังก์ชันสำหรับอัปเดตมวลของ Player
    private void UpdatePlayerMass()
    { 
        if (rb == null) return;

        float baseMass = 1f; // มวลเริ่มต้นของ Player
        float massPerObject = 1f; // มวลที่เพิ่มขึ้นต่อวัตถุ 1 ชิ้น

        // คำนวณมวลใหม่
        rb.mass = baseMass + (absorbedObjects.Count * massPerObject);

        Debug.Log("อัปเดตมวลของ Player: " + rb.mass);

        Debug.Log("UpdatePlayerMass ถูกเรียก");  // เพิ่มบรรทัดนี้เพื่อตรวจสอบ

        if (rb == null) return;

        rb.mass = baseMass + (absorbedObjects.Count * massPerObject);
        Debug.Log("อัปเดตมวลของ Player: " + rb.mass);
    }
    
    #region <UpdatePlayerCollider> //อัปเดตขนาดของ CircleCollider2D
    private void UpdatePlayerCollider() //อัปเดตขนาดของ CircleCollider2D
    {
        // คำนวณขนาดใหม่ตามน้ำหนัก
        float newScale = 1f + (weight / 100f); // เพิ่มขนาดตามน้ำหนัก
        transform.localScale = new Vector3(newScale, newScale, 1f);

        // ถ้ามี Collider2D เช่น CircleCollider2D
        CircleCollider2D collider = GetComponent<CircleCollider2D>();
        if (collider != null)
        {
            collider.radius = initialColliderRadius * newScale; // อัปเดตรัศมี Collider
        }
        
        CircleCollider2D playerCollider = GetComponent<CircleCollider2D>();

        if (playerCollider != null)
        {
            float initialRadius = 0.5f; // ขนาด Collider เริ่มต้น

            if (absorbedObjects.Count == 0) 
            {
                playerCollider.radius = initialRadius; // 📌 ถ้าไม่มี Obj ที่ดูดติด กลับไปขนาดเริ่มต้น
            }
            else
            {
                float maxDistance = 0f;

                foreach (GameObject obj in absorbedObjects)
                {
                    float distance = Vector3.Distance(transform.position, obj.transform.position);
                    if (distance > maxDistance)
                    {
                        maxDistance = distance;
                    }
                }

                float newRadius = maxDistance + 0.3f;
                playerCollider.radius = Mathf.Max(newRadius, initialRadius);
            }

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

    
    #region <OnDrawGizmos> //Editor แสดงวงกลมระยะการดูดวัตถุ
    private void OnDrawGizmos() //Editor แสดงวงกลมระยะการดูดวัตถุ
    {
        if (absorbPoint == null) return; // ตรวจสอบว่า absorbPoint ไม่เป็น null

        // แสดงระยะการดูดวัตถุใน Scene
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(absorbPoint.position, absorbRange);
    }
    #endregion
    
    //--------------------------------------------------------------------------------------------------------
    
    [Header("Hp Settings")]
    [SerializeField] public Image[] hpImages; // อ้างอิงถึงจุดรูปภาพ HP ทั้งหมด
    [SerializeField] int hp; // จำนวน HP เริ่มต้นของผู้เล่น
    [SerializeField] private int maxHp = 4; // กำหนดค่า HP สูงสุดของ Player

    
    [Header("Trash Can Settings")]
    //[SerializeField] private TrashCan trashCan; // อ้างอิงถึง TrashCan ที่แยกออกมา
    
    [Header("Lose Game Settings")]
    [SerializeField] private GameObject loseCanvas; // Canvas Lose
    [SerializeField] private GameObject gamePlayCanvas; // Canvas Game Play
    
    
    #region <UpdateAbsorbCountUI> //อัปเดต UI ของจำนวนวัตถุที่ผู้เล่นดูดมา
    private void UpdateAbsorbCountUI() //อัปเดต UI ของจำนวนวัตถุที่ผู้เล่นดูดมา
    {
        // ซ่อน UI ทั้งหมดก่อน
        foreach (Image img in absorbCountImages)
        {
            img.enabled = false; // ซ่อนทุกภาพ
        }

        // เปิด UI ตามจำนวนวัตถุที่ดูดได้
        for (int i = 0; i < absorbedObjects.Count && i < absorbCountImages.Length; i++)
        {
            absorbCountImages[i].enabled = true;
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
    }
    #endregion
    
    public int GetHealth()
    {
        return hp; // ส่งค่าปัจจุบันของ HP
    }
    
    #region <UpdateHPUI> //อัปเดต UI ของ HP ของผู้เล่น
    private void UpdateHPUI() //อัปเดต UI ของ HP ของผู้เล่น
    {
        for (int i = 0; i < hpImages.Length; i++)
        {
            if (i < hp)
            {
                hpImages[i].gameObject.SetActive(true); // เปิด GameObject
            }
            else
            {
                hpImages[i].gameObject.SetActive(false); // ปิด GameObject
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

        // เรียกใช้เสียง Game Over
        AudioManager.Instance.PlayGameOverSound();
        
        Debug.Log("Game Over! HP หมด.");
    }
    #endregion
    
    // ฟังก์ชัน Reset สถานะของ Player
    public void ResetPlayerStats()
    {
        hp = maxHp; // รีเซ็ตค่า HP
        UpdateHPUI(); // อัปเดต UI HP ใหม่

        // เคลียร์วัตถุที่ดูดซับมา
        foreach (GameObject obj in absorbedObjects)
        {
            if (obj != null)
            {
                obj.transform.SetParent(null); // ปลดจาก Parent
                Destroy(obj); // ทำลายวัตถุ
            }
        }

        absorbedObjects.Clear(); // ล้างลิสต์ให้ว่างเปล่า

        UpdatePlayerMass(); // อัปเดตมวลหลังจากรีเซ็ต
        UpdatePlayerCollider(); // อัปเดต Collider หลังจากรีเซ็ต

        Debug.Log("ResetPlayerStats() -> Clear absorbedObjects, Count: " + absorbedObjects.Count);
    }



    public void ResetPlayerMass()
    {
        rb.mass = 1f; // รีเซ็ตมวลเป็นค่าตั้งต้น
    }

    public void ResetPlayerCollider()
    {
        CircleCollider2D collider = GetComponent<CircleCollider2D>();
        if (collider != null)
        {
            collider.radius = initialColliderRadius; // รีเซ็ตขนาด Collider
        }
    }

    public void ClearAbsorbedObjects()
    {
        foreach (GameObject obj in absorbedObjects)
        {
            if (obj != null)
            {
                obj.transform.SetParent(null);
                Destroy(obj);
            }
        }

        absorbedObjects.Clear(); // ล้างลิสต์ให้ว่างเปล่า
        UpdatePlayerMass(); // อัปเดตมวลใหม่
        UpdatePlayerCollider(); // อัปเดต Collider ใหม่

        Debug.Log("ClearAbsorbedObjects() -> Cleared objects, Count: " + absorbedObjects.Count);
    }

    
}

