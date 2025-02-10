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

    // ฟังก์ชันที่เรียกเมื่อกดปุ่ม Play
    public void OnPlayButtonClicked()
    {
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
        if (homePageCanvas != null)
        {
            homePageCanvas.SetActive(false);
        }

        if (how_to_play_Panel != null)
        {
            how_to_play_Panel.SetActive(true);
        }
    }

    // ฟังก์ชันที่เรียกเมื่อกดปุ่ม Exit
    public void OnExitButtonClicked()
    {
        Debug.Log("Exit button clicked! Exiting the game...");
        Application.Quit(); // ออกจากเกม (ใช้งานได้เฉพาะ Build จริง)
    }
}