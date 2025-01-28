using UnityEngine;
using System.Collections;

public class PlatformWeightController : MonoBehaviour
{
    /*[SerializeField] public float targetWeight; // น้ำหนักที่ต้องการเพื่อให้ Platform เลื่อนลงมา
    [SerializeField] public float moveSpeed; // ความเร็วในการเลื่อนลง
    public Transform platform; // อ้างอิง Platform
    public Vector3 targetPosition; // ตำแหน่งสุดท้ายที่ Platform จะเลื่อนลงไป

    private float currentWeight = 0f; // น้ำหนักปัจจุบันของ Player บน Platform
    private bool isMoving = false; // เช็คว่า Platform กำลังเลื่อนลงอยู่หรือไม่

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            // เพิ่มน้ำหนักเมื่อ Player หรือวัตถุเข้ามาอยู่บน Platform
            currentWeight += rb.mass;

            // ตรวจสอบว่าน้ำหนักเท่ากับ targetWeight หรือไม่
            if (Mathf.Approximately(currentWeight, targetWeight))
            {
                isMoving = true; // เริ่มเลื่อน Platform ลงมา
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            // ลดน้ำหนักเมื่อ Player หรือวัตถุออกจาก Platform
            currentWeight -= rb.mass;
            currentWeight = Mathf.Max(currentWeight, 0); // ป้องกันน้ำหนักติดลบ
        }
    }

    private void Update()
    {
        if (isMoving && platform != null)
        {
            // ค่อยๆ เลื่อน Platform ไปยังตำแหน่งที่กำหนด
            platform.position = Vector3.MoveTowards(platform.position, targetPosition, moveSpeed * Time.deltaTime);

            // หยุดการเลื่อนเมื่อถึงตำแหน่งเป้าหมาย
            if (platform.position == targetPosition)
            {
                isMoving = false; // หยุดเลื่อน
            }
        }
    }*/
    
    [Header("Platform Settings")]
    [SerializeField] private int objectsInPlatform = 0; // จำนวนวัตถุที่อยู่ในPlatform
    [SerializeField] private int maxObjectsInPlatform = 4; // จำนวนวัตถุที่ต้องการเพื่อทำให้Platformลื่อนลง

    [Header("UI Settings")]
    [SerializeField] private GameObject[] uiPoints; // Array ของ GameObject ที่จะแสดงตำแหน่งการวางวัตถุ
    [SerializeField] private Color greenColor; // สีเขียวที่จะแสดงเมื่อวางวัตถุ
    [SerializeField] private Color grayColor; // สีเทาที่จะแสดงเมื่อเริ่มต้น

    [Header("Lift Movement Settings")]
    [SerializeField] private GameObject lift; // ลิฟต์ที่เราจะเลื่อน
    [SerializeField] private float PlatformMoveSpeed = 2f; // ความเร็วในการเลื่อนPlatform

    private void Start()
    {
        objectsInPlatform = 0; // รีเซ็ตค่าเริ่มต้น
        ResetUIPoints(); // รีเซ็ต UI จุดทั้งหมด
    }

    private void ResetUIPoints()
    {
        // รีเซ็ตสถานะของ UI จุดทั้งหมด (ซ่อนจุดทั้งหมดและตั้งค่าสีเป็นสีเทา)
        foreach (var point in uiPoints)
        {
            point.SetActive(false); // ซ่อนจุด
            var spriteRenderer = point.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                spriteRenderer.color = grayColor; // ตั้งค่าสีเป็นสีเทา
            }
        }
    }

    private void SetUIPoint(int index)
    {
        // แสดงจุดที่ถูกวางวัตถุ
        if (index < uiPoints.Length)
        {
            var point = uiPoints[index];
            point.SetActive(true); // แสดงจุด
            var spriteRenderer = point.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                spriteRenderer.color = greenColor; // เปลี่ยนสีเป็นสีเขียว
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // ตรวจสอบว่ามีวัตถุที่สามารถวางในPlatformหรือไม่
        if (collision.gameObject.CompareTag("TrashObject"))
        {
            AddObjectToLift(collision.gameObject); // เพิ่มวัตถุเข้าในPlatform
        }
    }

    public void AddObjectToLift(GameObject obj)
    {
        // เพิ่มจำนวนวัตถุในลPlatform
        objectsInPlatform++;

        if (objectsInPlatform <= uiPoints.Length)
        {
            // เปลี่ยนสถานะของ UI จุดให้เป็น Active และเปลี่ยนสี
            SetUIPoint(objectsInPlatform - 1);
        }

        if (objectsInPlatform >= maxObjectsInPlatform)
        {
            // เมื่อครบจำนวนวัตถุในPlatform จะทำให้Platformเลื่อนลง
            StartCoroutine(MoveLiftDown());

            // รีเซ็ตจำนวนวัตถุในPlatform
            objectsInPlatform = 0;
            ResetUIPoints(); // รีเซ็ต UI จุดทั้งหมด
        }

        // ลบวัตถุหลังจากวางลงในลิฟต์
        Destroy(obj);
    }

    private IEnumerator MoveLiftDown()
    {
        // เลื่อนลPlatformลง
        Vector3 targetPosition = lift.transform.position + new Vector3(0, -7.3f, 0); // เลื่อนลง 5 หน่วย (ปรับได้ตามต้องการ)

        while (Vector3.Distance(lift.transform.position, targetPosition) > 0.1f)
        {
            lift.transform.position = Vector3.MoveTowards(lift.transform.position, targetPosition, PlatformMoveSpeed * Time.deltaTime);
            yield return null; // จะหยุดการทำงานชั่วขณะในแต่ละเฟรม
        }

        // เมื่อเลื่อนลงถึงที่แล้ว ให้Platformยุดที่ตำแหน่งใหม่
        lift.transform.position = targetPosition;
    }
}

