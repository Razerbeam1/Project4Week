using UnityEngine;
using UnityEngine.SceneManagement;

public class win : MonoBehaviour
{
    public void RestartGame()
    {
        // รีเซ็ต Time.timeScale เป็นค่าเริ่มต้น (1)
        Time.timeScale = 1f;

        // โหลดฉากปัจจุบันใหม่
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
}