using UnityEngine;
using UnityEngine.UI;

public class credit : MonoBehaviour
{
    public Image backgroundImage; // ใส่ Image ที่ต้องการเปลี่ยนรูป
    public Sprite[] backgroundSprites; // ใส่รูปภาพใน Inspector
    private int currentIndex = 0;
    
    void Start()
    {
        if (backgroundSprites.Length > 0)
        {
            InvokeRepeating("ChangeSprite", 0f, 0.25f); // เปลี่ยนภาพทุก 0.25 วินาที (4 FPS)
        }
    }

    void Update()
    {
        
    }

    // ฟังก์ชันที่ใช้เปลี่ยน Sprite ของ Background ทุกๆ 0.25 วินาที
    void ChangeSprite()
    {
        if (backgroundSprites.Length == 0) return;
        
        backgroundImage.sprite = backgroundSprites[currentIndex];
        currentIndex = (currentIndex + 1) % backgroundSprites.Length;
    }
}
