using UnityEngine;

public class SignController : MonoBehaviour
{
    [SerializeField] private GameObject signObject; // วัตถุที่จะเปิด/ปิด (เช่น ป้าย sign)
    private bool isPlayerNear = false; // สถานะว่าผู้เล่นอยู่ใกล้หรือไม่

    // เมื่อผู้เล่นชนกับคันโยก
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) // ตรวจสอบว่าชนกับผู้เล่น
        {
            isPlayerNear = true; // ตั้งค่าสถานะผู้เล่นใกล้
            Debug.Log("ผู้เล่นอยู่ใกล้sign");
            OpenSign(); // เรียกใช้ฟังก์ชันเปิด sign
        }
    }

    // เมื่อผู้เล่นออกจากการชนคันโยก
    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) // ตรวจสอบว่าผู้เล่นออกจากการชน
        {
            isPlayerNear = false; // ตั้งค่าสถานะผู้เล่นไม่ใกล้
            Debug.Log("ผู้เล่นออกจาsign");
            CloseSign(); // เรียกใช้ฟังก์ชันปิด sign
        }
    }

    // ฟังก์ชันสำหรับเปิด sign
    private void OpenSign()
    {
        if (signObject != null)
        {
            signObject.SetActive(true); // เปิด sign
            Debug.Log("เปิด sign");
        }
    }

    // ฟังก์ชันสำหรับปิด sign
    private void CloseSign()
    {
        if (signObject != null)
        {
            signObject.SetActive(false); // ปิด sign
            Debug.Log("ปิด sign");
        }
    }
}