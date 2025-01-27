using UnityEngine;

public class SpikeController : MonoBehaviour
{
    public GameObject[] spikes; // อาร์เรย์ของหนามทั้ง 4 อัน
    public float interval = 4f; // ช่วงเวลาที่หนามจะสลับกันออก (4 วินาที)

    private int currentSpikeIndex = 0; // ตำแหน่งหนามที่กำลังออก
    private float timer = 0f; // ตัวจับเวลาสำหรับควบคุมการสลับหนาม

    void Start()
    {
        // ปิดการแสดงผลหนามทั้งหมดเมื่อเริ่มต้น
        foreach (GameObject spike in spikes)
        {
            spike.SetActive(false);
        }
    }

    void Update()
    {
        timer += Time.deltaTime; // เพิ่มเวลาตามเฟรมที่ผ่านไป

        if (timer >= interval)
        {
            timer = 0f; // รีเซ็ตตัวจับเวลา

            // ปิดหนามปัจจุบัน
            spikes[currentSpikeIndex].SetActive(false);

            // เปลี่ยนไปที่หนามอันถัดไป
            currentSpikeIndex = (currentSpikeIndex + 1) % spikes.Length;

            // เปิดการแสดงผลหนามอันถัดไป
            spikes[currentSpikeIndex].SetActive(true);
        }
    }
}

