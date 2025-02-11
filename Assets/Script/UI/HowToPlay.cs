using UnityEngine;
using UnityEngine.UI; // ✅ นำเข้า UI ให้ใช้งาน Button และ Image ได้

public class HowToPlay : MonoBehaviour
{
    [SerializeField] private Image displayImage; // UI Image ที่ใช้แสดงรูป
    [SerializeField] private Sprite[] images; // เก็บรูปภาพทั้งหมด
    [SerializeField] private UnityEngine.UI.Button nextButton; // ✅ ใช้ UnityEngine.UI.Button
    [SerializeField] private UnityEngine.UI.Button goBackButton; // ✅ ปุ่ม Go Back
    
    public GameObject pausePanel;
    public PauseGame pauseGameScript; // ตัวแปรเก็บสคริป PauseGame

    private int currentIndex = 0; // ตำแหน่งรูปปัจจุบัน

    private void Start()
    {
        UpdateImage();

        if (nextButton != null)
            nextButton.onClick.AddListener(NextImage);

        if (goBackButton != null)
            goBackButton.onClick.AddListener(CloseHowToPlay); // ✅ ปิดหน้า HowToPlay
        
        // ค้นหา PauseGame ใน scene
        pauseGameScript = FindObjectOfType<PauseGame>();
        
        // ปิด PauseGame เมื่อเปิด HowToPlay
        if (pauseGameScript != null)
        {
            pauseGameScript.enabled = false;
        }
    }

    private void NextImage()
    {
        currentIndex = (currentIndex + 1) % images.Length;
        UpdateImage();
    }

    private void UpdateImage()
    {
        displayImage.sprite = images[currentIndex]; // อัปเดตรูปภาพ
    }

    private void CloseHowToPlay()
    {
        gameObject.SetActive(false); // ✅ ปิดหน้า HowToPlay
        pausePanel.SetActive(true);
        
        // เปิด PauseGame กลับเมื่อปิด HowToPlay
        if (pauseGameScript != null)
        {
            pauseGameScript.enabled = true;
        }
        
    }
}