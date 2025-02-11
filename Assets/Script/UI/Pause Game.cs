using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseGame : MonoBehaviour
{
    public Image backgroundImage; // ใส่ Image ที่ต้องการเปลี่ยนรูป
    public Sprite[] backgroundSprites; // ใส่รูปภาพใน Inspector
    public GameObject pausePanel; // Panel ที่ใช้แสดงเมื่อเกมหยุด
    private int currentIndex = 0;
    
    public GameObject howToPlayPanel; // ✅ เพิ่ม GameObject สำหรับหน้า HowToPlay
    

    void Start()
    {
        pausePanel.SetActive(false); // เริ่มต้นด้วยการซ่อน Panel Pause
        howToPlayPanel.SetActive(false); // ✅ ซ่อนหน้า HowToPlay ตอนเริ่มเกม

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
    
    private void TogglePause()
    {
        if (pausePanel.activeSelf)
        {
            ResumeGame();
        }
        else
        {
            PauseGameSystem();
        }
    }

    private void PauseGameSystem()
    {
        Time.timeScale = 0f;
        pausePanel.SetActive(true);
    }

    private void ResumeGame()
    {
        Time.timeScale = 1f;
        pausePanel.SetActive(false);
    }

    void ChangeSprite()
    {
        if (backgroundSprites.Length == 0) return;
        
        backgroundImage.sprite = backgroundSprites[currentIndex];
        currentIndex = (currentIndex + 1) % backgroundSprites.Length;
    }
    
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    public void OpenHowToPlay() // ✅ เปิดหน้า HowToPlay
    {
        pausePanel.SetActive(false); // ซ่อนหน้า Pause
        howToPlayPanel.SetActive(true); // เปิดหน้า HowToPlay
    }
    
}
