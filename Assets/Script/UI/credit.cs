using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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

    // ฟังก์ชันที่ใช้เปลี่ยน Sprite ของ Background ทุกๆ 0.25 วินาที
    void ChangeSprite()
    {
        if (backgroundSprites.Length == 0) return;
        
        backgroundImage.sprite = backgroundSprites[currentIndex];
        currentIndex = (currentIndex + 1) % backgroundSprites.Length;
    }
    
    [Header("Home page")] 
    [SerializeField] private GameObject Home_page;
    
    [Header("Credit")]
    [SerializeField] private GameObject credit_Panel;
    
    public void OnHome_pageButtonClicked()
    {
        credit_Panel.SetActive(false);
        Home_page.SetActive(true);
        // โหลด Scene ปัจจุบันใหม่เพื่อรีเซ็ตเกม
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    
    
}
