using UnityEngine;
using UnityEngine.UI; // ✅ นำเข้า UI ให้ใช้งาน Button และ Image ได้

public class HowToPlayHome : MonoBehaviour
{
    [SerializeField] private Image displayImage; // UI Image ที่ใช้แสดงรูป
    [SerializeField] private Sprite[] images; // เก็บรูปภาพทั้งหมด
    [SerializeField] private UnityEngine.UI.Button nextButton; // ✅ ใช้ UnityEngine.UI.Button
    [SerializeField] private UnityEngine.UI.Button goBackButton; // ✅ ปุ่ม Go Back

    private int currentIndex = 0; // ตำแหน่งรูปปัจจุบัน

    private void Start()
    {
        UpdateImage();

        if (nextButton != null)
            nextButton.onClick.AddListener(NextImage);

        if (goBackButton != null)
            goBackButton.onClick.AddListener(CloseHowToPlay); // ✅ ปิดหน้า HowToPlay
    }

    private void NextImage()
    {
        currentIndex = (currentIndex + 1) % images.Length;
        UpdateImage();
        
        // ✅ เล่นเสียงกดปุ่ม
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayHowToPlaySound();
        }
    }

    private void UpdateImage()
    {
        displayImage.sprite = images[currentIndex]; // อัปเดตรูปภาพ
    }

    private void CloseHowToPlay()
    {
        gameObject.SetActive(false); // ✅ ปิดหน้า HowToPlay
        
        // ✅ เล่นเสียงกดปุ่ม
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayHowToPlaySound();
        }
        
    }
}