using UnityEngine;
using UnityEngine.SceneManagement; 

public class lose : MonoBehaviour
{
    public GameObject Main_menu;
    public GameObject Lose;
    
    public void RestartGame()
    {
        // รีเซ็ต Time.timeScale เป็นค่าเริ่มต้น (1)
        Time.timeScale = 1f;

        // โหลดฉากปัจจุบันใหม่
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    public void Mainmenu ()
    {
        Main_menu.SetActive(true);
        Lose.SetActive(false);
    }
    
    
}