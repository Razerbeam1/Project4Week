using UnityEngine;
using System.Collections;

public class Magnet : MonoBehaviour
{
    [SerializeField] private float pullForce;  // ความแรงในการดูด
    [SerializeField] private float maxDistance;  // ระยะทางที่แม่เหล็กสามารถดูด Player ได้
    [SerializeField] private float releaseDistance;  // ระยะทางเมื่อปล่อย Player ออก
    [SerializeField] private Transform player;  // ตัวแปรเพื่อเก็บตำแหน่งของ Player

    private bool isPlayerNear = false;  // ตัวแปรเช็คว่า Player อยู่ใกล้แม่เหล็กหรือไม่
    private bool isMagnetActive = true;  // เช็คสถานะการดูดหรือปล่อย
    private bool isCoolingDown = false; // ตัวแปรใช้เช็คว่าแม่เหล็กอยู่ในโหมด cooldown หรือไม่

    void Update()
    {
        // ตรวจสอบระยะห่างระหว่างแม่เหล็กและ Player
        float distance = Vector3.Distance(transform.position, player.position);

        // ถ้า Player อยู่ในระยะที่แม่เหล็กดูด
        if (distance <= maxDistance && isMagnetActive && !isCoolingDown)
        {
            isPlayerNear = true;
        }
        else
        {
            isPlayerNear = false;
        }

        // ถ้า Player อยู่ใกล้แม่เหล็กและสถานะการดูดเปิดใช้งาน
        if (isPlayerNear && isMagnetActive)
        {
            // ดึง Player เข้ามาใกล้แม่เหล็ก
            Vector3 direction = (transform.position - player.position).normalized;
            player.position = Vector3.MoveTowards(player.position, transform.position, pullForce * Time.deltaTime);
        }

        // ปล่อย Player เมื่อกด W
        if (Input.GetKeyDown(KeyCode.W) && !isCoolingDown)
        {
            StartCoroutine(ReleasePlayer());  // เริ่ม Coroutine หยุดการดูด
        }
    }

    // Coroutine ที่จะหยุดการดูดเป็นเวลา 3 วินาที
    private IEnumerator ReleasePlayer()
    {
        isMagnetActive = false;  // ปิดการดูด
        isCoolingDown = true;  // เปิดโหมด cooldown

        // รอ 3 วินาที
        yield return new WaitForSeconds(3f);

        isMagnetActive = true;  // เปิดการดูดใหม่
        isCoolingDown = false;  // ปิดโหมด cooldown
    }
}
