using UnityEngine;
using UnityEngine.SceneManagement; // สำหรับการจัดการ Scene

public class lose : MonoBehaviour
{
    public void RestartGame()
    {
        // รีเซ็ต Time.timeScale เป็นค่าเริ่มต้น (1)
        Time.timeScale = 1f;

        // โหลดฉากปัจจุบันใหม่
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    // ฟังก์ชันที่เรียกเมื่อกดปุ่ม Exit
    public void OnExitButtonClicked()
    {
        Debug.Log("Exit button clicked! Exiting the game...");
        Application.Quit(); // ออกจากเกม (ใช้งานได้เฉพาะ Build จริง)
    }
}