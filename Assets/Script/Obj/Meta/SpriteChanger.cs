using UnityEngine;

public class SpriteChanger : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer; // ตัว SpriteRenderer ของ GameObject
    [SerializeField] private Sprite[] sprites; // Array ของ Sprite ที่ต้องการสลับ
    [SerializeField] private float changeInterval = 0.5f; // ระยะเวลาในการเปลี่ยน Sprite (วินาที)

    private int currentSpriteIndex = 0; // Index ของ Sprite ปัจจุบัน
    private float timer; // ตัวจับเวลา

    private void Start()
    {
        if (spriteRenderer == null)
        {
            // หากไม่ได้กำหนด SpriteRenderer ใน Inspector ให้ใช้ตัวที่อยู่ใน GameObject นี้
            spriteRenderer = GetComponent<SpriteRenderer>();
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
    }

    private void ChangeSprite()
    {
        if (sprites.Length == 0) return; // หากไม่มี Sprite ให้หยุดทำงาน

        // เปลี่ยน Sprite ของ SpriteRenderer
        spriteRenderer.sprite = sprites[currentSpriteIndex];

        // เพิ่ม Index และวนกลับไปที่ 0 หากถึงจุดสิ้นสุด
        currentSpriteIndex = (currentSpriteIndex + 1) % sprites.Length;
    }
}