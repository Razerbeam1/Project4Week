using UnityEngine;
using UnityEngine.UI;

public class SoundController : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Button soundButton;  // ปุ่มที่คลิกเพื่อแสดง/ซ่อนบาร์
    [SerializeField] private Slider volumeSlider;  // สไลด์บาร์สำหรับปรับระดับเสียง
    [SerializeField] private GameObject volumeBar;  // บาร์เสียงที่ใช้แสดงระดับเสียง (สามารถเป็น Panel หรือ Slider)
    [SerializeField] private Image volumeImage;  // รูปภาพที่เปลี่ยนตามระดับเสียง

    [Header("Volume Image")]
    [SerializeField] private Sprite[] volumeImages;  // รูปภาพสำหรับระดับเสียง (เช่น จากเงียบไปดัง)
    
    private bool isVolumeBarActive = false;  // ควบคุมการแสดง/ซ่อนบาร์เสียง

    private void Start()
    {
        // กำหนดให้ปุ่มเมื่อคลิกจะเปิด/ปิดบาร์เสียง
        soundButton.onClick.AddListener(ToggleVolumeBar);

        // ตั้งค่าเริ่มต้นให้บาร์เสียงปิด
        volumeBar.SetActive(false);

        // ตั้งค่าเริ่มต้นของ slider
        volumeSlider.value = AudioListener.volume;
        volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
    }

    // ฟังก์ชันเพื่อเปิด/ปิดบาร์เสียง
    public void ToggleVolumeBar()
    {
        isVolumeBarActive = !isVolumeBarActive;

        // แสดง/ซ่อนบาร์เสียง
        volumeBar.SetActive(isVolumeBarActive);
    }

    // ฟังก์ชันที่จะเรียกเมื่อมีการเปลี่ยนค่า Slider
    public void OnVolumeChanged(float value)
    {
        // อัปเดตระดับเสียงของระบบ
        AudioListener.volume = value;

        // อัปเดตรูปภาพตามระดับเสียง
        UpdateVolumeImage(value);
    }

    // อัปเดตรูปภาพตามระดับเสียง
    public void UpdateVolumeImage(float value)
    {
        int index = Mathf.FloorToInt(value * (volumeImages.Length - 1));
        volumeImage.sprite = volumeImages[index];
    }
}
