using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseGame : MonoBehaviour
{
    public Image backgroundImage; // ใส่ Image ที่ต้องการเปลี่ยนรูป
    public Sprite[] backgroundSprites; // ใส่รูปภาพใน Inspector
    public GameObject pausePanel; // Panel ที่ใช้แสดงเมื่อเกมหยุด
    private int currentIndex = 0;

    public GameObject mainmenu;

    void Start()
    {
        pausePanel.SetActive(false); // เริ่มต้นด้วยการซ่อน Panel Pause
        if (backgroundSprites.Length > 0)
        {
            InvokeRepeating("ChangeSprite", 0f, 0.25f); // เปลี่ยนภาพทุก 0.25 วินาที (4 FPS)
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) // กด ESC เพื่อหยุดเกม
        {
            Debug.Log("ESC Key Pressed");  // ทดสอบดูว่าปุ่ม ESC ถูกกดจริงหรือไม่
            TogglePause();
        }
    }

    // ฟังก์ชันสำหรับสลับสถานะการหยุดเกม
    private void TogglePause()
    {
        if (pausePanel.activeSelf) // ถ้า Panel Pause เปิดอยู่
        {
            ResumeGame();
        }
        else // ถ้า Panel Pause ปิดอยู่
        {
            PauseGameSystem();
        }
    }

    // ฟังก์ชันที่หยุดเกม
    private void PauseGameSystem()
    {
        Time.timeScale = 0f; // หยุดเกม
        pausePanel.SetActive(true); // เปิดแสดง Panel Pause
    }

    // ฟังก์ชันที่เริ่มเกม
    private void ResumeGame()
    {
        Time.timeScale = 1f; // เริ่มเกมใหม่
        pausePanel.SetActive(false); // ซ่อน Panel Pause
    }

    // ฟังก์ชันที่ใช้เปลี่ยน Sprite ของ Background ทุกๆ 0.25 วินาที
    void ChangeSprite()
    {
        if (backgroundSprites.Length == 0) return;
        
        backgroundImage.sprite = backgroundSprites[currentIndex];
        currentIndex = (currentIndex + 1) % backgroundSprites.Length;
    }
    
    
    public void RestartGame()
    {
        // รีเซ็ต Time.timeScale เป็นค่าเริ่มต้น (1)
        Time.timeScale = 1f;

        // โหลดฉากปัจจุบันใหม่
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Mainmenu()
    {
        pausePanel.SetActive(false);
        mainmenu.SetActive(true);
        
    }
}