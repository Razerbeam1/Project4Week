using System.Collections;
using UnityEngine;

public class FloorBreakable : MonoBehaviour
{
    [SerializeField] private float timeToBreak; // เวลาที่ต้องใช้ในการทำให้พื้นพัง 10f
    [SerializeField] private float weightThreshold; // น้ำหนักที่ทำให้พื้นพังทันที 100f
    [SerializeField] private Rigidbody2D floorRigidbody; // Rigidbody ของพื้น
    [SerializeField] private bool destroyAfterFall = true; // ถ้า true พื้นจะถูกลบหลังตกลง

    private float timePlayerOnFloor = 0f; // เวลาที่ผู้เล่นยืนบนพื้น
    private bool isPlayerOnFloor = false; // ตรวจสอบว่าผู้เล่นอยู่บนพื้นหรือไม่
    private bool isBroken = false; // ตรวจสอบว่าพื้นพังแล้วหรือยัง

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

    private void Update()
    {
        if (isPlayerOnFloor && !isBroken)
        {
            // เพิ่มเวลาที่ผู้เล่นอยู่บนพื้น
            timePlayerOnFloor += Time.deltaTime;

            // ถ้าเวลาที่ผู้เล่นอยู่บนพื้นเกินกำหนด ให้พื้นพัง
            if (timePlayerOnFloor >= timeToBreak)
            {
                BreakFloor();
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isPlayerOnFloor = true;

            // ตรวจสอบน้ำหนักของผู้เล่น
            Rigidbody2D playerRigidbody = collision.gameObject.GetComponent<Rigidbody2D>();
            if (playerRigidbody != null && playerRigidbody.mass >= weightThreshold)
            {
                BreakFloor(); // พื้นพังทันทีถ้าผู้เล่นน้ำหนักเกิน
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isPlayerOnFloor = false;
            timePlayerOnFloor = 0f; // รีเซ็ตเวลาที่ผู้เล่นยืนบนพื้น
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
