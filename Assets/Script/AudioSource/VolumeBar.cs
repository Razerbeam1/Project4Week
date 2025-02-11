using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class VolumeBar : MonoBehaviour, IPointerDownHandler, IScrollHandler
{
    [SerializeField] private Image[] volumeStages; // รูปภาพ BG 4 รูป
    [SerializeField] private RectTransform handle; // จุด Handle ที่ลากขึ้นลง
    [SerializeField] private RectTransform barBackground; // พื้นหลังของ Bar
    [SerializeField] private AudioSource audioSource; // ตัวควบคุมเสียง
    [SerializeField] private GameObject volumeBar; // ตัวแทนของ Volume Bar
    [SerializeField] private GameObject volumeIcon; // ไอคอนที่ใช้เปิด/ปิด Volume Bar

    [SerializeField] private AudioManager audioManager; // เชื่อม AudioManager
    
    private float minY, maxY;
    private bool isBarActive = false; // ตัวแปรบอกว่า Bar เปิดอยู่หรือไม่

    private void Start()
    {
        minY = barBackground.rect.yMin;
        maxY = barBackground.rect.yMax;

        // เริ่มต้นด้วยการตั้งระดับเสียงจาก PlayerPrefs และซ่อน Volume Bar
        SetVolume(PlayerPrefs.GetFloat("Volume", 1.0f)); 
        volumeBar.SetActive(isBarActive); // ซ่อน Volume Bar ในตอนแรก
    }

    // ฟังก์ชันสำหรับการคลิกที่ไอคอนเพื่อเปิด/ปิด Bar
    public void OnPointerDown(PointerEventData eventData)
    {
        ToggleVolumeBar(); // เรียกใช้ฟังก์ชันสลับสถานะการเปิด/ปิด Bar
    }

    // ฟังก์ชันสำหรับการกลิ้งลูกกลิ้งเมาส์
    public void OnScroll(PointerEventData eventData)
    {
        float scrollDelta = eventData.scrollDelta.y; // ทิศทางการกลิ้ง (ขึ้นหรือลง)
        float volumeChange = scrollDelta > 0 ? 0.05f : -0.05f; // เพิ่มหรือลด 5% ตามทิศทางการกลิ้ง

        // ปรับระดับเสียงใหม่
        float newVolume = Mathf.Clamp01(AudioListener.volume + volumeChange);
        SetVolume(newVolume); // เรียกฟังก์ชันที่ใช้ปรับระดับเสียง
    }

    // ฟังก์ชันสำหรับเปิด/ปิด Bar
    private void ToggleVolumeBar()
    {
        isBarActive = !isBarActive; // สลับสถานะของ Bar
        volumeBar.SetActive(isBarActive); // แสดงหรือซ่อน Volume Bar

        if (!isBarActive)
        {
            // ถ้า Volume Bar ปิดแล้ว ให้บันทึกสถานะเสียงไว้
            PlayerPrefs.SetFloat("Volume", AudioListener.volume);
        }
    }

    private void SetVolume(float value)
    {
        AudioListener.volume = value; // ปรับเสียงหลัก
        if (audioSource) audioSource.volume = value; // ปรับเสียงใน AudioSource (ถ้ามี)

        // ปรับแสดงภาพตามระดับเสียง
        for (int i = 0; i < volumeStages.Length; i++)
        {
            volumeStages[i].gameObject.SetActive(value > (i * 0.25f)); // แสดง/ซ่อนรูปภาพตามระดับเสียง
        }

        if (handle)
            handle.anchoredPosition = new Vector2(handle.anchoredPosition.x, Mathf.Lerp(minY, maxY, value)); // ปรับตำแหน่ง Handle

        
        // ตั้งค่าระดับเสียงใน AudioManager
        audioManager.SetVolume(value);
        PlayerPrefs.SetFloat("Volume", value); // บันทึกระดับเสียงไว้
    }
}
