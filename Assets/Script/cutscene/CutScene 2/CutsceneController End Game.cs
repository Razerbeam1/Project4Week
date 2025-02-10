using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CutsceneControllerEndGame : MonoBehaviour
{
    public Image[] cutsceneImages; // รูปภาพทั้งหมด 4 รูป
    [SerializeField] float fadeDuration; // ระยะเวลาการเฟด
    [SerializeField] float delayBetweenImages; // เวลาหน่วงก่อนแสดงภาพถัดไป

    [SerializeField] private GameObject panelCutsceneENDGame; // Panel ของ Cutscene
    [SerializeField] private GameObject WinPanel; // Panel ของ Credit ที่จะเปิดหลังจาก Cutscene เสร็จ
    [SerializeField] private GameObject panelGameSystem; // Panel ของ Game System
    [SerializeField] private GameObject panel_UIGame_Playe; // Panel ของ UIGame_Playe

    private bool hasWinPanelBeenOpened = false;

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

    void Update()
    {
        if (WinPanel != null)
        {
            if (!WinPanel.activeSelf)
            {
                Debug.LogError("⚠️ มีบางอย่างไปปิด WinPanel หลังจากเปิด!", WinPanel);

                // ลองเช็คว่าใครเป็นคนปิดมัน
                GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();
                foreach (var obj in allObjects)
                {
                    if (obj.activeSelf == false && obj == WinPanel)
                    {
                        Debug.LogError($"🔎 {obj.name} ถูกปิดโดยไม่ทราบสาเหตุ!", obj);
                    }
                }
            }
        }
    }


    private IEnumerator PlayCutscene()
    {
        for (int i = 0; i < cutsceneImages.Length; i++)
        {
            yield return StartCoroutine(FadeIn(cutsceneImages[i]));
            yield return new WaitForSeconds(delayBetweenImages);
        }
        
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
        Debug.Log("EndCutscene2 Called!");
    
        if (WinPanel != null)
        {
            Debug.Log("WinPanel is not null, activating now...");
            ShowWinPanel();
        }
        else
        {
            Debug.LogError("WinPanel is NULL! Please assign it in the Inspector.");
        }
    
        panelCutsceneENDGame?.SetActive(false);
        panelGameSystem?.SetActive(false);
        panel_UIGame_Playe?.SetActive(false);
    }

    private void ShowWinPanel()
    {
        if (WinPanel != null)
        {
            WinPanel.SetActive(true);
            hasWinPanelBeenOpened = true;
            Debug.Log("✅ WinPanel Activated: " + WinPanel.activeSelf);
        }
        else
        {
            Debug.LogError("❌ WinPanel is NULL! ไม่สามารถเปิดได้");
        }
    }
}

