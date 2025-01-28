using UnityEngine;

public class SpriteChanger : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer; // ตัว SpriteRenderer ของ GameObject
    [SerializeField] private Sprite[] sprites; // Array ของ Sprite ที่ต้องการสลับ
    [SerializeField] private float changeInterval = 0.5f; // ระยะเวลาในการเปลี่ยน Sprite (วินาที)

    [SerializeField]public float mass; // น้ำหนักของวัตถุแต่ละชิ้น
    
    private int currentSpriteIndex = 0; // Index ของ Sprite ปัจจุบัน
    private float timer; // ตัวจับเวลา
    
    private bool isInTrapZone = false; // เช็คว่าอยู่ในพื้นที่ Trap หรือไม่
    private Vector2 escapeDirection; // ทิศทางที่จะเคลื่อนออกจาก Trap

    private Rigidbody2D rb2d; // Rigidbody2D ของ GameObject เพื่อควบคุมการเคลื่อนที่

    private void Start()
    {
        if (spriteRenderer == null)
        {
            // หากไม่ได้กำหนด SpriteRenderer ใน Inspector ให้ใช้ตัวที่อยู่ใน GameObject นี้
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
        
        rb2d = GetComponent<Rigidbody2D>(); // เรียกใช้งาน Rigidbody2D

        if (rb2d == null)
        {
            Debug.LogError("Rigidbody2D is missing from the GameObject. Please add a Rigidbody2D.");
        }
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= changeInterval)
        {
            ChangeSprite();
            timer = 0f; // รีเซ็ตตัวจับเวลา
        }
        
        // ถ้าอยู่ในพื้นที่ Trap ให้เคลื่อนออกจาก Trap
        if (isInTrapZone)
        {
            MoveAwayFromTrap();
        }
    }

    private void ChangeSprite()
    {
        if (sprites.Length == 0) return; // หากไม่มี Sprite ให้หยุดทำงาน

        // เปลี่ยน Sprite ของ SpriteRenderer
        spriteRenderer.sprite = sprites[currentSpriteIndex];

        // เพิ่ม Index และวนกลับไปที่ 0 หากถึงจุดสิ้นสุด
        currentSpriteIndex = (currentSpriteIndex + 1) % sprites.Length;
    }
    
    
    private void MoveAwayFromTrap()
    {
        if (rb2d != null)
        {
            // เคลื่อนที่ออกห่างจาก Trap โดยใช้ Force หรือการเคลื่อนที่แบบตำแหน่ง
            rb2d.velocity = escapeDirection * 5f; // ใช้ความเร็วเพื่อเคลื่อนที่ออกจาก Trap (สามารถปรับค่าตัวเลขได้)

            // หยุดการเคลื่อนที่หากพ้นจาก Trap
            // ตรวจสอบว่าห่างจาก Trap มากพอหรือไม่
            float distanceToTrap = Vector2.Distance(transform.position, escapeDirection + (Vector2)transform.position);
            if (distanceToTrap > 2f) // ถ้าห่างออกจาก Trap มากพอ
            {
                isInTrapZone = false; // หยุดการเคลื่อนที่
                rb2d.velocity = Vector2.zero; // หยุดการเคลื่อนที่
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // เช็คว่าชนกับ Trap หรือไม่
        if (other.CompareTag("Trap"))
        {
            // คำนวณทิศทางการหลีกหนีจาก Trap
            escapeDirection = (transform.position - other.transform.position).normalized; // คำนวณทิศทางที่ถูกต้อง

            // ทำเครื่องหมายว่าอยู่ในพื้นที่ Trap
            isInTrapZone = true;

            Debug.Log("Entered Trap Zone. Moving away...");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // เมื่อออกจากพื้นที่ Trap
        if (other.CompareTag("Trap"))
        {
            // หยุดการเคลื่อนที่
            isInTrapZone = false;
            rb2d.velocity = Vector2.zero; // หยุดการเคลื่อนที่
            Debug.Log("Exited Trap Zone. Stopping movement.");
        }
    }
}