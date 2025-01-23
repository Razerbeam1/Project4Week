using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject homePageCanvas; // Canvas Home Page
    [SerializeField] private GameObject cutscenePanel; // Panel สำหรับ Cutscene

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

    // ฟังก์ชันที่เรียกเมื่อกดปุ่ม Exit
    public void OnExitButtonClicked()
    {
        Debug.Log("Exit button clicked! Exiting the game...");
        Application.Quit(); // ออกจากเกม (ใช้งานได้เฉพาะ Build จริง)
    }
}