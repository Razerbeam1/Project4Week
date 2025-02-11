using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CutsceneController : MonoBehaviour
{
    public Image[] cutsceneImages; // รูปภาพทั้งหมด 4 รูป
    [SerializeField] float fadeDuration; // ระยะเวลาการเฟด
    [SerializeField] float delayBetweenImages; // เวลาหน่วงก่อนแสดงภาพถัดไป

    [SerializeField] private GameObject panelCutsceneStarGame; // Panel ของ Cutscene
    [SerializeField] private GameObject panelGameSystem; // Panel ของ Game System
    [SerializeField] private GameObject panel_UIGame_Playe; // Panel ของ UIGame_Playe

    private static bool isCutscenePlaying = false; // ตัวแปรป้องกันการเล่นซ้ำ

    private void Start()
    {
        // ตั้งค่าเริ่มต้นให้รูปทั้งหมดโปร่งใส
        foreach (Image img in cutsceneImages)
        {
            Color tempColor = img.color;
            tempColor.a = 0f;
            img.color = tempColor;
        }

        StartCoroutine(PlayCutscene());
    }


    private IEnumerator PlayCutscene()
    {
        // เริ่มเล่นเสียง Cutscene เมื่อเริ่มเล่น Cutscene
        AudioManager.Instance.PlayCutsceneSound(); 
        
        for (int i = 0; i < cutsceneImages.Length; i++)
        {
            yield return StartCoroutine(FadeIn(cutsceneImages[i]));
            yield return new WaitForSeconds(delayBetweenImages);
        }

        // เมื่อ Cutscene จบ → ปิด Panel Cutscene และเปิด Panel Game System
        EndCutscene();
    }

    private IEnumerator FadeIn(Image image)
    {
        float elapsedTime = 0f;
        Color tempColor = image.color;

        while (elapsedTime < fadeDuration)
        {
            tempColor.a = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);
            image.color = tempColor;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        tempColor.a = 1f;
        image.color = tempColor;
    }

    private void EndCutscene()
    {
        panelCutsceneStarGame.SetActive(false); // ปิด Cutscene
        panelGameSystem.SetActive(true); // เปิด Game System
        panel_UIGame_Playe.SetActive(true); // เปิด UIGame_Playe
        
        // หยุดเสียงเมื่อจบ Cutscene
        AudioManager.Instance.mainAudioSource.Stop(); // หยุดเสียง
        
        // เรียกใช้งานเสียงพื้นหลังของเกม
        AudioManager.Instance.PlayBackgroundMusic(); // เล่นเสียงพื้นหลังเกม
    }
}

