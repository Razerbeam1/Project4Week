using UnityEngine;

public class RotatingDoor : MonoBehaviour
{
    [SerializeField] private float slideSpeed = 5f;       // ความเร็วในการเลื่อนประตู
    [SerializeField] private Vector3 slideOffset;         // ระยะทางที่ประตูจะเลื่อนออกไปทางซ้าย
    [SerializeField] private GameObject button;           // ปุ่มที่ Player ต้องชน

    private Vector3 initialPosition;                      // ตำแหน่งเริ่มต้นของประตู
    private Vector3 targetPosition;                       // ตำแหน่งที่ประตูจะเลื่อนถึง
    private bool isButtonActivated = false;               // เช็คว่าปุ่มถูกกดหรือไม่

    private void Start()
    {
        initialPosition = transform.position;             // บันทึกตำแหน่งเริ่มต้นของประตู
        targetPosition = initialPosition + slideOffset;   // คำนวณตำแหน่งเป้าหมาย
    }

    private void Update()
    {
        if (isButtonActivated)
        {
            // เลื่อนประตูไปยังตำแหน่งเป้าหมายอย่างนุ่มนวล
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, slideSpeed * Time.deltaTime);
        }
        else
        {
            // เลื่อนประตูกลับไปยังตำแหน่งเริ่มต้น
            transform.position = Vector3.MoveTowards(transform.position, initialPosition, slideSpeed * Time.deltaTime);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // ถ้า Player ชนกับปุ่ม
        if (collision.gameObject.CompareTag("Player"))
        {
            isButtonActivated = true;  // เปิดใช้งานปุ่ม
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // ถ้า Player ออกจากการชนกับปุ่ม
        if (collision.gameObject.CompareTag("Player"))
        {
            isButtonActivated = false;  // ปิดการใช้งานปุ่ม
        }
    }
}