using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CutsceneControllerEndGame : MonoBehaviour
{
    public Image[] cutsceneImages; // รูปภาพทั้งหมด 4 รูป
    [SerializeField] float fadeDuration; // ระยะเวลาการเฟด
    [SerializeField] float delayBetweenImages; // เวลาหน่วงก่อนแสดงภาพถัดไป

    public GameObject cutscenePanel; // Panel ของ Cutscene
    public GameObject WinPanel; // Panel ของ Credit ที่จะเปิดหลังจาก Cutscene เสร็จ
    
    
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
        for (int i = 0; i < cutsceneImages.Length; i++)
        {
            yield return StartCoroutine(FadeIn(cutsceneImages[i]));
            yield return new WaitForSeconds(delayBetweenImages);
        }
        
        // เมื่อ Cutscene จบ → ปิด Panel Cutscene และเปิด Panel Game System
        EndCutscene2();
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
    
    private void EndCutscene2()
    {
        WinPanel.SetActive(true);
        cutscenePanel.SetActive(false);
        
    }

}
