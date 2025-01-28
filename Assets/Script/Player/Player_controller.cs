using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("Control Settings")]
    [SerializeField] private float speed; // ความเร็วการเคลื่อนที่
    //[SerializeField] private float jumpForce; // แรงกระโดด
    [SerializeField] private Transform absorbPoint; // จุดที่ใช้ดูดวัตถุ
    [SerializeField] private float absorbRange; // ระยะการดูดวัตถุ
    [SerializeField] private GameObject[] absorbableObjects; // วัตถุที่สามารถดูดได้
    [SerializeField] private float sizeIncreaseFactor; // ปริมาณการขยายตัวของ Player ต่อวัตถุหนึ่งชิ้น
    [SerializeField] private Image[] absorbCountImages; // Array ของรูปภาพ UI
    [SerializeField] private int maxAbsorbableObjects; // จำนวนวัตถุสูงสุดที่ดูดได้
    
    
    public float moveSpeed = 5f; // ความเร็วในการเคลื่อนที่
    public float groundStickForce = 10f; // แรงยึดติดกับพื้น
    public float edgeSnapDistance = 0.2f; // ระยะห่างที่ใช้ตรวจจับขอบพื้น
    private Rigidbody2D rb2d; // ตัวควบคุม Rigidbody2D
    private bool isOnGround = false; // ตรวจสอบว่าอยู่บนพื้นหรือไม่
    private Vector2 groundNormal; // ทิศทาง Normal ของพื้น
    
    

    private Rigidbody2D rb;
    private bool isGrounded = false;
    private List<GameObject> absorbedObjects = new List<GameObject>(); // เก็บวัตถุที่ถูกดูด

    private float initialScale; // ขนาดเริ่มต้นของ Player
    //private int jumpCount = 0; // ตัวนับจำนวนการกระโดด
    //[SerializeField] private int maxJumpCount; // จำนวนการกระโดดสูงสุด
    
    
    [Header("Weight Settings")]
    [SerializeField] private float weightIncreasePerObject; // น้ำหนักที่เพิ่มต่อวัตถุที่ดูด
    private float weight; // น้ำหนักของ Player
    [SerializeField] private float baseWeight; // น้ำหนักเริ่มต้นของ Player

    
    [Header("Hp Settings")]
    [SerializeField] public Image[] hpImages; // อ้างอิงถึงจุดรูปภาพ HP ทั้งหมด
    [SerializeField] int hp; // จำนวน HP เริ่มต้นของผู้เล่น

    /*[Header("Mana Settings")]
    [SerializeField] private Image[] manaImages; // รูปภาพของ Bar มานา
    [SerializeField] private float manaRechargeTime; // เวลาที่ใช้ในการรีเซ็ตมานาจุดละ 1 จุด
    [SerializeField] private float manaConsumptionTime; // เวลาที่ใช้ในการลดมานาจุดละ 1 จุด
    private int currentMana; // มานาปัจจุบัน
    private float manaTimer; // ตัวจับเวลาสำหรับการลดมานา
    private bool isAbsorbing; // ตรวจสอบว่าผู้เล่นกำลังกดดูดวัตถุอยู่หรือไม่*/
    
    [Header("Trash Can Settings")]
    [SerializeField] private TrashCan trashCan; // อ้างอิงถึง TrashCan ที่แยกออกมา
    
    [Header("Lose Game Settings")]
    [SerializeField] private GameObject loseCanvas; // Canvas Lose
    [SerializeField] private GameObject gamePlayCanvas; // Canvas Game Play
    
    
    private float initialColliderRadius; //รัศมีเริ่มต้นของ Collider
    
    
   
    
    private bool isTouchingWall; // เช็คว่า Player กำลังสัมผัสกำแพงหรือไม่
    private bool isClimbing; // เช็คว่า Player กำลังปีนกำแพงหรือไม่
    private float fallingSpeed = 50f; // ความเร็วในการตกลงมา
    private float climbSpeed = 5f; // ความเร็วในการปีน
    
    
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        initialScale = transform.localScale.x; // เก็บขนาดเริ่มต้นไว้
        
        weight = baseWeight; // ตั้งค่าน้ำหนักเริ่มต้น
        UpdateAbsorbCountUI(); // อัปเดต UI เริ่มต้น
        UpdateHPUI(); // อัปเดต UI ตอนเริ่มเกม
        
        //currentMana = manaImages.Length; // เริ่มต้นมานาเต็ม
        //UpdateManaUI();
        
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
        
        rb2d = GetComponent<Rigidbody2D>();
        if (rb2d == null)
        {
            Debug.LogError("Rigidbody2D is missing from the Player object! Please add a Rigidbody2D.");
        }
    }

    private void Update()
    {
        HandleMovement();
        //HandleJump();
        //HandleAbsorption();
        HandleReleaseObjects();
        HandleAbsorptionWithMana();
        //RechargeMana();
    }
    
    #region <HandleMovement> // ควบคุมการเคลื่อนที่
    private void HandleMovement()
    {
        /*float moveInput = 0f;

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
        }*/
        
        float targetVelocityX = 0f;
    float targetVelocityY = 0f;

    // ตรวจสอบการกดปุ่มในแนวแกน X (ซ้าย/ขวา)
    if (Input.GetKey(KeyCode.A))
    {
        targetVelocityX = -1f; // เคลื่อนไปทางซ้าย
    }
    else if (Input.GetKey(KeyCode.D))
    {
        targetVelocityX = 1f; // เคลื่อนไปทางขวา
    }

    // ถ้า Player กำลังสัมผัสกำแพงและกดปุ่ม W ให้สามารถปีนขึ้นไปได้
    if (isTouchingWall && Input.GetKey(KeyCode.W))
    {
        isClimbing = true; // เริ่มปีนกำแพง
        targetVelocityY = climbSpeed; // ให้ Player ปีนขึ้น
    }
    else if (isTouchingWall && Input.GetKey(KeyCode.S))
    {
        isClimbing = true; // เริ่มปีนกำแพงลง
        targetVelocityY = -climbSpeed; // ให้ Player ปีนลง
    }
    else
    {
        isClimbing = false; // หยุดปีนกำแพงเมื่อไม่ได้กด W หรือ S
    }

    // ถ้ากำลังปีนกำแพงจะไม่อนุญาตให้เคลื่อนที่ในแนว X
    if (isClimbing)
    {
        targetVelocityX = 0f; // หยุดการเคลื่อนไหวในแนว X
    }

    // หาก Player กำลังไม่สัมผัสกำแพงและปีนอยู่ ให้ตกลงมาเร็ว
    if (!isTouchingWall && isClimbing)
    {
        targetVelocityY = -fallingSpeed; // ทำให้มันตกลงมาเร็ว
    }

    // หาก Player กำลังอยู่บนพื้นและไม่ได้ปีนกำแพง
    if (isOnGround && !isClimbing)
    {
        // ตรวจสอบการกดปุ่มในแนวแกน Y (ขึ้น/ลง)
        if (Input.GetKey(KeyCode.W))
        {
            targetVelocityY = 1f; // เคลื่อนไปข้างบน
        }
        else if (Input.GetKey(KeyCode.S))
        {
            targetVelocityY = -1f; // เคลื่อนไปข้างล่าง
        }

        // รับการเคลื่อนที่จากผู้เล่น
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        // คำนวณทิศทางการเคลื่อนที่
        Vector2 inputDirection = new Vector2(horizontal, vertical);

        // ปรับการเคลื่อนที่ให้สอดคล้องกับพื้น
        Vector2 moveDirection = AdjustMovementToGround(inputDirection) * moveSpeed;

        // ตั้งค่าความเร็วใหม่
        targetVelocityX = moveDirection.x;
        targetVelocityY = moveDirection.y;
    }

    // ปรับความเร็วตามน้ำหนัก
    float adjustedSpeed = moveSpeed / (1 + weight / 100);

    // คำนวณความเร็วในแนว X และ Y โดยไม่ใช้การสมูท
    float newVelocityX = targetVelocityX * adjustedSpeed;

    // แก้ไขการคำนวณ Y velocity ให้เหมาะสมกับการตกและการปีน
    float newVelocityY;
    if (isClimbing)
    {
        newVelocityY = targetVelocityY;
    }
    else if (isOnGround)
    {
        newVelocityY = targetVelocityY;
    }
    else
    {
        newVelocityY = targetVelocityY;
    }

    // อัปเดตความเร็วใหม่ในแกน X และ Y
    rb2d.velocity = new Vector2(newVelocityX, newVelocityY);

    // การหมุนตัวและการหันหน้า
    if (targetVelocityX != 0)
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
        
    }
    #endregion 
    
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
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = false;
        }
    }



    
    
    
    #region <HandleJump> //ควบคุมการกระโดด
    /*private void HandleJump() //ควบคุมการกระโดด
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
    }*/
    #endregion
     
    #region <HandleAbsorptionWithMana> //กดปุ่มซ้ายค้างเพื่อดูดวัตถุพร้อมกับการใช้มานาในการดูด
    private void HandleAbsorptionWithMana() //กดปุ่มซ้ายค้างเพื่อดูดวัตถุพร้อมกับการใช้มานาในการดูด
    {
        /*if (Input.GetMouseButton(0)) // กดคลิกซ้ายค้าง
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
        }*/
        
        AbsorbObjects(); // ทำการดูดวัตถุ
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
    /*private void OnCollisionEnter2D(Collision2D collision) //มีการชนกับวัตถุชนิด 2D โดยใช้เพื่อตรวจสอบว่าผู้เล่นอยู่บนพื้นหรือไม่ และตรวจจับกับดัก
    {
        // เช็คว่าชนพื้นหรือไม่
        if (collision.contacts[0].normal.y > 0.5f)
        {
            isGrounded = true;
            jumpCount = 0; // รีเซ็ตจำนวนการกระโดดเมื่อสัมผัสพื้น
        }
    }*/
    #endregion
    
    #region <OnCollisionExit2D> //ผู้เล่นไม่อยู่บนพื้นแล้ว
    /*private void OnCollisionExit2D(Collision2D collision) //ผู้เล่นไม่อยู่บนพื้นแล้ว
    {
        // เมื่อไม่อยู่บนพื้น (อาจจะขึ้นจากพื้นหรืออยู่ในอากาศ)
        isGrounded = false;
    }*/
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
    
    #region <UpdateHPUI> //อัปเดต UI ของ HP ของผู้เล่น
    private void UpdateHPUI() //อัปเดต UI ของ HP ของผู้เล่น
    {
        for (int i = 0; i < hpImages.Length; i++)
        {
            if (i < hp)
            { 
                //hpImages[i].color = Color.green; // สีปกติ
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
    /*private void RechargeMana() //รีเซ็ตมานา (เพิ่มมานา) เมื่อไม่ได้ทำการดูดวัตถุ   
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
    }*/
    #endregion

    #region <UpdateManaUI>
    /*private void UpdateManaUI() //อัปเดต UI ของมานา
    {
        // แสดง/ซ่อน Bar มานา ตามจำนวนมานาปัจจุบัน
        for (int i = 0; i < manaImages.Length; i++)
        {
            manaImages[i].enabled = i < currentMana;
        }
    }*/
    #endregion
    
    
}
