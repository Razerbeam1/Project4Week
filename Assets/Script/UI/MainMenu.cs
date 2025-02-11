using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject homePageCanvas; // Canvas Home Page
    [SerializeField] private GameObject cutscenePanel; // Panel สำหรับ Cutscene

    [Header("Credit")] 
    [SerializeField] private GameObject credit_Panel;
    
    [Header("Credit")] 
    [SerializeField] private GameObject how_to_play_Panel;
    
    
    private void Start()
    {
        // เล่นเสียงหน้า Home Page เมื่อเข้าเมนู
        AudioManager.Instance.PlayHomePageSound();
    }

    // ฟังก์ชันที่เรียกเมื่อกดปุ่ม Play
    public void OnPlayButtonClicked()
    {
        AudioManager.Instance.PlayButtonClickSound(); // เล่นเสียงกดปุ่ม
        
        // หยุดเสียงหน้า Home Page
        AudioManager.Instance.mainAudioSource.Stop(); // หยุดเสียงของหน้า Home Page
        
        if (homePageCanvas != null)
        {
            homePageCanvas.SetActive(false); // ซ่อน Canvas Home Page
        }

        if (cutscenePanel != null)
        {
            cutscenePanel.SetActive(true); // แสดง Panel Cutscene
        }

        Debug.Log("Play button clicked! Showing Cutscene...");
    }

    public void OnCreditButtonClicked()
    {
        AudioManager.Instance.PlayButtonClickSound();
        
        if (homePageCanvas != null)
        {
            homePageCanvas.SetActive(false);
        }

        if (credit_Panel != null)
        {
            credit_Panel.SetActive(true);
        }
    }

    public void OnHow_to_playitButtonClicked()
    {
        AudioManager.Instance.PlayHowToPlaySound();
        
        if (how_to_play_Panel != null)
        {
            how_to_play_Panel.SetActive(true);
        }
    }

    // ฟังก์ชันที่เรียกเมื่อกดปุ่ม Exit
    public void OnExitButtonClicked()
    {
        AudioManager.Instance.PlayButtonClickSound();
        Debug.Log("Exit button clicked! Exiting the game...");
        Application.Quit(); // ออกจากเกม (ใช้งานได้เฉพาะ Build จริง)
    }
}