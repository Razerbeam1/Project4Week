using System.Collections;
using UnityEngine;

public class BreakableFloor : MonoBehaviour
{
    [SerializeField] private Rigidbody2D floorRigidbody; // Rigidbody ของพื้น
    [SerializeField] private bool destroyAfterFall = true; // ถ้า true พื้นจะถูกลบหลังตกลง

    private int jumpCount = 0; // จำนวนครั้งที่ผู้เล่นกระโดดบนพื้น
    private bool isPlayerOnFloor = false; // ตรวจสอบว่าผู้เล่นอยู่บนพื้นหรือไม่
    private bool isBroken = false; // ตรวจสอบว่าพื้นพังแล้วหรือยัง
    private int maxJumpsToBreak = 2; // จำนวนกระโดดที่ทำให้พื้นพังทันที

    private void Start()
    {
        if (floorRigidbody == null)
        {
            floorRigidbody = GetComponent<Rigidbody2D>();
        }

        // ล็อคพื้นในตำแหน่งเดิมจนกว่าจะพัง
        if (floorRigidbody != null)
        {
            floorRigidbody.bodyType = RigidbodyType2D.Static;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isBroken)
        {
            isPlayerOnFloor = true;

            // เพิ่มจำนวนกระโดดทุกครั้งที่ผู้เล่นชนพื้น
            jumpCount++;
            Debug.Log("Player landed on floor. Jump count: " + jumpCount);

            // ถ้าผู้เล่นกระโดดครบตามที่กำหนด ให้พื้นพัง
            if (jumpCount >= maxJumpsToBreak)
            {
                BreakFloor();
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isPlayerOnFloor = false;
            Debug.Log("Player left the floor.");
        }
    }

    private void BreakFloor()
    {
        if (isBroken) return;

        isBroken = true; // ตั้งค่าสถานะว่าพื้นพังแล้ว
        Debug.Log("Floor has broken!");

        // ปลดล็อก Rigidbody ให้พื้นตกลง
        if (floorRigidbody != null)
        {
            floorRigidbody.bodyType = RigidbodyType2D.Dynamic;
        }

        // ถ้ากำหนดให้ลบพื้นหลังจากตก ให้ทำลบพื้น
        if (destroyAfterFall)
        {
            StartCoroutine(DestroyFloorAfterFall());
        }
    }

    private IEnumerator DestroyFloorAfterFall()
    {
        yield return new WaitForSeconds(1f); // รอให้พื้นตกลงจนหมด 1 วินาที 
        Destroy(gameObject); // ลบพื้นออกจากฉาก
    }
}
