using System.Collections;
using UnityEngine;

public class FloorBreakable : MonoBehaviour
{
    [SerializeField] private float timeToBreak; // เวลาที่ต้องใช้ในการทำให้พื้นพัง
    [SerializeField] private float weightThreshold; // น้ำหนักที่ทำให้พื้นพังทันที
    [SerializeField] private Rigidbody2D floorRigidbody; // Rigidbody ของพื้น
    [SerializeField] private float resetDelay; // เวลาที่ใช้รอก่อนรีเซ็ตกลับตำแหน่งเดิม

    private float timePlayerOnFloor = 0f; // เวลาที่ผู้เล่นยืนบนพื้น
    private bool isPlayerOnFloor = false; // ตรวจสอบว่าผู้เล่นอยู่บนพื้นหรือไม่
    private bool isBroken = false; // ตรวจสอบว่าพื้นพังแล้วหรือยัง

    private Vector3 initialPosition; // ตำแหน่งเริ่มต้นของพื้น
    private Quaternion initialRotation; // การหมุนเริ่มต้นของพื้น

    private void Start()
    {
        // เก็บตำแหน่งและการหมุนเริ่มต้น
        initialPosition = transform.position;
        initialRotation = transform.rotation;

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

        // รอรีเซ็ตพื้นกลับตำแหน่งเดิม
        StartCoroutine(ResetFloor());
    }

    private IEnumerator ResetFloor()
    {
        yield return new WaitForSeconds(resetDelay); // รอให้พื้นตกลงไปก่อน

        // รีเซ็ตตำแหน่งและการหมุน
        transform.position = initialPosition;
        transform.rotation = initialRotation;

        // รีเซ็ต Rigidbody กลับไปเป็น Static
        if (floorRigidbody != null)
        {
            floorRigidbody.bodyType = RigidbodyType2D.Static;
            floorRigidbody.velocity = Vector2.zero; // รีเซ็ตความเร็วของ Rigidbody
            floorRigidbody.angularVelocity = 0f;   // รีเซ็ตการหมุนของ Rigidbody
        }

        // รีเซ็ตสถานะพื้น
        isBroken = false;
        timePlayerOnFloor = 0f;

        Debug.Log("Floor has been reset to its original position.");
    }
}
