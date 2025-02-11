using UnityEngine;
using System.Collections;

public class Navigator : MonoBehaviour
{
    [SerializeField] private GameObject[] objectsToToggle; // Object ที่จะเปิด-ปิดแบบไล่เรียงกัน
    [SerializeField] private float blinkInterval = 0.5f; // ระยะเวลาที่ Object กระพริบ
    [SerializeField] private Player_controller2 playerHealth; // อ้างอิงไปยัง PlayerHealth Script
    [SerializeField] private int maxActiveObjects = 4; // จำนวน Object ที่เปิดพร้อมกันสูงสุด

    private bool isNavigating = false;

    private void Start()
    {
        // ปิด Object ทั้งหมดก่อนเริ่ม
        foreach (var obj in objectsToToggle)
        {
            obj.SetActive(false);
        }
    }

    private void Update()
    {
        // ตรวจสอบ HP ของ Player และเปิดใช้งาน Navigator เมื่อ HP ลดลงเหลือ 2 หรือไม่
        if (playerHealth.GetHealth() <= 2 && !isNavigating)
        {
            isNavigating = true;
            StartCoroutine(BlinkObjects()); // เริ่มกระพริบ Object
        }
        // ปิดการทำงานของ Navigator เมื่อ HP มากกว่า 2
        else if (playerHealth.GetHealth() > 2 && isNavigating)
        {
            isNavigating = false;
            StopCoroutine(BlinkObjects()); // หยุดการกระพริบ Object
            foreach (var obj in objectsToToggle)
            {
                obj.SetActive(false); // ปิด Object ทั้งหมด
            }
        }
    }

    private IEnumerator BlinkObjects()
    {
        int index = 0;
        while (isNavigating) // ใช้ตัวแปร isNavigating ในการเช็คว่า Navigator ยังทำงานหรือไม่
        {
            // ปิดทั้งหมดก่อน
            foreach (var obj in objectsToToggle)
            {
                obj.SetActive(false);
            }

            // เปิด Object ไล่เรียงกันตาม maxActiveObjects
            for (int i = 0; i < maxActiveObjects; i++)
            {
                int activeIndex = (index + i) % objectsToToggle.Length;
                objectsToToggle[activeIndex].SetActive(true);
            }

            yield return new WaitForSeconds(blinkInterval);

            index = (index + 1) % objectsToToggle.Length; // วนลำดับไปเรื่อยๆ
        }
    }
}
