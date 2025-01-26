using System.Collections;
using UnityEngine;

public class TrashCan : MonoBehaviour
{
    [Header("Trash Can Settings")]
    [SerializeField] private int objectsInTrashCan = 0; // จำนวนวัตถุที่อยู่ในถังขยะ
    [SerializeField] private int maxObjectsInTrashCan = 2; // จำนวนวัตถุที่ต้องการเพื่อทำให้ถังขยะกระตุก
    [Header("UI Settings")]
    [SerializeField] private GameObject[] uiPoints; // Array ของ GameObject ที่จะเป็นตัวแทนการทิ้งวัตถุ
    [SerializeField] private Color greenColor; // สีเขียวที่จะแสดงเมื่อทิ้งวัตถุลงถังขยะ
    [SerializeField] private Color grayColor; // สีเทาที่จะแสดงเมื่อเริ่มต้น
    [Header("Door Settings")]
    [SerializeField] private GameObject door; // ประตูที่จะเลื่อนขึ้น
    [SerializeField] private float doorMoveSpeed; // ความเร็วในการเลื่อนประตู

    private void Start()
    {
        objectsInTrashCan = 0; // รีเซ็ตค่าเริ่มต้น
        // รีเซ็ตสถานะของ UI จุดทั้งหมด (ทำให้ซ่อนจุดทั้งหมดและตั้งค่าสีเป็นสีเทา)
        ResetUIPoints();
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
        // แสดงจุดที่ถูกทิ้งวัตถุ
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ตรวจสอบว่ามีวัตถุที่สามารถทิ้งในถังขยะหรือไม่
        if (collision.CompareTag("TrashObject"))
        {
            AddObjectToTrash();
            Destroy(collision.gameObject); // ลบวัตถุหลังจากทิ้งในถังขยะ
        }
    }

    public void AddObjectToTrash()
    {
        // เพิ่มจำนวนวัตถุในถังขยะ
        objectsInTrashCan++;

        if (objectsInTrashCan <= uiPoints.Length)
        {
            // เปลี่ยนสถานะของ UI จุดให้เป็น Active และเปลี่ยนสี
            SetUIPoint(objectsInTrashCan - 1);
        }

        if (objectsInTrashCan >= maxObjectsInTrashCan)
        {
            // เมื่อครบจำนวนวัตถุในถังขยะจะทำให้ประตูเลื่อนขึ้น
            StartCoroutine(MoveDoorUp());

            // รีเซ็ตจำนวนวัตถุในถังขยะ
            objectsInTrashCan = 0;
            ResetUIPoints(); // รีเซ็ต UI จุดทั้งหมด
        }
    }

    private IEnumerator MoveDoorUp()
    {
        // เลื่อนประตูขึ้น
        Vector3 targetPosition = door.transform.position + new Vector3(0, 5f, 0); // เลื่อนขึ้น 5 หน่วย (ปรับได้ตามต้องการ)

        while (Vector3.Distance(door.transform.position, targetPosition) > 0.1f)
        {
            door.transform.position = Vector3.MoveTowards(door.transform.position, targetPosition, doorMoveSpeed * Time.deltaTime);
            yield return null;
        }

        // เมื่อเลื่อนขึ้นถึงที่แล้ว ให้ประตูหยุดที่ตำแหน่งใหม่
        door.transform.position = targetPosition;
    }
}
