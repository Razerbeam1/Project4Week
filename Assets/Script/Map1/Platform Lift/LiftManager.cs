using System.Collections;
using UnityEngine;

public class LiftManager : MonoBehaviour
{
    [Header("Lift Settings")]
    [SerializeField] private int objectsInLift = 0; // จำนวนวัตถุที่อยู่ในลิฟต์
    [SerializeField] private int maxObjectsInLift = 4; // จำนวนวัตถุที่ต้องการเพื่อทำให้ลิฟต์เลื่อนลง

    [Header("UI Settings")]
    [SerializeField] private GameObject[] uiPoints; // Array ของ GameObject ที่จะแสดงตำแหน่งการวางวัตถุ
    [SerializeField] private Color greenColor; // สีเขียวที่จะแสดงเมื่อวางวัตถุ
    [SerializeField] private Color grayColor; // สีเทาที่จะแสดงเมื่อเริ่มต้น

    [Header("Lift Movement Settings")]
    [SerializeField] private GameObject lift; // ลิฟต์ที่เราจะเลื่อน
    [SerializeField] private float liftMoveSpeed = 2f; // ความเร็วในการเลื่อนลิฟต์
    
    [SerializeField] private SignController signController; // อ้างอิงถึง SignController


    private void Start()
    {
        objectsInLift = 0; // รีเซ็ตค่าเริ่มต้น
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
        // ตรวจสอบว่ามีวัตถุที่สามารถวางในลิฟต์หรือไม่
        if (collision.gameObject.CompareTag("TrashObject"))
        {
            AddObjectToLift(collision.gameObject); // เพิ่มวัตถุเข้าในลิฟต์
        }
    }

    public void AddObjectToLift(GameObject obj)
    {
        // เพิ่มจำนวนวัตถุในลิฟต์
        objectsInLift++;

        if (objectsInLift <= uiPoints.Length)
        {
            // เปลี่ยนสถานะของ UI จุดให้เป็น Active และเปลี่ยนสี
            SetUIPoint(objectsInLift - 1);
        }

        if (objectsInLift >= maxObjectsInLift)
        {
            // เมื่อครบจำนวนวัตถุในลิฟต์ จะทำให้ลิฟต์เลื่อนลง
            StartCoroutine(MoveLiftDown());

            // รีเซ็ตจำนวนวัตถุในลิฟต์
            objectsInLift = 0;
            ResetUIPoints(); // รีเซ็ต UI จุดทั้งหมด
        }

        // ลบวัตถุหลังจากวางลงในลิฟต์
        Destroy(obj);
    }

    private IEnumerator MoveLiftDown()
    {
        // เลื่อนลิฟต์ลง
        Vector3 targetPosition = lift.transform.position + new Vector3(0, -7.3f, 0); // เลื่อนลง 5 หน่วย (ปรับได้ตามต้องการ)

        while (Vector3.Distance(lift.transform.position, targetPosition) > 0.1f)
        {
            lift.transform.position = Vector3.MoveTowards(lift.transform.position, targetPosition, liftMoveSpeed * Time.deltaTime);
            yield return null; // จะหยุดการทำงานชั่วขณะในแต่ละเฟรม
        }

        // เมื่อเลื่อนลงถึงที่แล้ว ให้ลิฟต์หยุดที่ตำแหน่งใหม่
        lift.transform.position = targetPosition;
        
        // ปิดสคริปต์ SignController
        if (signController != null)
        {
            Debug.Log("SignController is being disabled");
            signController.enabled = false; // ปิดการทำงานของ SignController
        }
        else
        {
            Debug.Log("SignController is not assigned");
        }
    }
}
