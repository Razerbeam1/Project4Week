using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverController : MonoBehaviour
{
    [SerializeField] float moveSpeed; // ความเร็วในการเลื่อน Dor
    private bool isPlayerNear = false; // ตรวจสอบว่าผู้เล่นอยู่ใกล้หรือไม่
    private bool isLeverPulled = false; // ตรวจสอบว่าคันโยกถูกโยกแล้วหรือไม่
    private bool hasRotated = false; // ตรวจสอบว่าคันโยกหมุนไปแล้วหรือยัง
    private bool isDorAtFinalPosition = false; // ตรวจสอบว่า Dor เลื่อนถึงปลายทางแล้วหรือยัง

    private Vector3 originalPosition; // ตำแหน่งเริ่มต้นของคันโยก
    private Vector3 dorFinalPosition; // ตำแหน่งสุดท้ายที่ Dor จะเลื่อนไป

    private float rotationAmount = -45f; // จำนวนองศาที่จะหมุนคันโยก

    [SerializeField] GameObject Dor; // อ้างอิงถึง GameObject Dor ที่จะเลื่อนลง
    [SerializeField] float moveDistance; // ระยะที่ Dor จะเลื่อนลง

    private List<GameObject> collectedObjects = new List<GameObject>(); // เก็บ obj ที่โยนใส่คันโยก
    private int requiredTrashObjects = 3; // จำนวน obj ที่ต้องการ

    void Start()
    {
        originalPosition = transform.position; // บันทึกตำแหน่งเริ่มต้นของคันโยก
        dorFinalPosition = new Vector3(Dor.transform.position.x, Dor.transform.position.y - moveDistance, Dor.transform.position.z); // ตำแหน่งปลายทางของ Dor
    }

    void Update()
    {
        // ตรวจสอบว่าผู้เล่นอยู่ใกล้และกดปุ่ม E หนึ่งครั้ง และมี obj ครบตามจำนวนที่กำหนด
        if (isPlayerNear && Input.GetKeyDown(KeyCode.E) && !isLeverPulled && !hasRotated && collectedObjects.Count >= requiredTrashObjects)
        {
            // หมุนคันโยกโดยใช้ rotationAmount
            transform.Rotate(0, 0, rotationAmount);
            // เริ่มเลื่อน Dor ลง
            isLeverPulled = true; // คันโยกถูกโยกแล้ว
            hasRotated = true; // คันโยกหมุนแล้ว
        }

        // ถ้าคันโยกถูกโยกแล้ว และประตูยังไม่ถึงตำแหน่งสุดท้าย ให้เลื่อน Dor ลง
        if (isLeverPulled && !isDorAtFinalPosition)
        {
            MoveDor(); // เรียกฟังก์ชันสำหรับเลื่อน Dor
        }
    }

    void MoveDor()
    {
        // เลื่อน Dor ลงไปทีละน้อยจนกว่าจะถึงตำแหน่งปลายทาง
        Dor.transform.position = Vector3.MoveTowards(Dor.transform.position, dorFinalPosition, moveSpeed * Time.deltaTime);

        // ตรวจสอบว่า Dor ถึงตำแหน่งปลายทางแล้วหรือยัง
        if (Dor.transform.position == dorFinalPosition)
        {
            isLeverPulled = false; // หยุดการเลื่อน Dor เมื่อถึงปลายทาง
            isDorAtFinalPosition = true; // ตั้งค่าว่า Dor ถึงปลายทางแล้ว
            Debug.Log("Dor เลื่อนลงจนสุดแล้ว");
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) // ตรวจสอบว่าชนกับผู้เล่น
        {
            isPlayerNear = true; // ตั้งค่าสถานะผู้เล่นใกล้
            Debug.Log("ผู้เล่นอยู่ใกล้คันโยก");
        }
        else if (collision.gameObject.CompareTag("TrashObject")) // ถ้าชนกับ TrashObject
        {
            AddTrashObject(collision.gameObject); // เรียกฟังก์ชันเพิ่ม TrashObject
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) // ตรวจสอบว่าผู้เล่นออกจากการชน
        {
            isPlayerNear = false; // ตั้งค่าสถานะผู้เล่นไม่ใกล้
            Debug.Log("ผู้เล่นออกจากการชนคันโยก");
        }
    }

    void AddTrashObject(GameObject trashObject)
    {
        if (collectedObjects.Count < requiredTrashObjects) // ตรวจสอบว่ายังเก็บ obj ได้อยู่หรือไม่
        {
            collectedObjects.Add(trashObject); // เพิ่ม obj ลงในอาร์เรย์
            Destroy(trashObject); // ลบ obj ออกจากเกม
            Debug.Log("เก็บ TrashObject แล้ว: " + collectedObjects.Count + "/" + requiredTrashObjects);

            if (collectedObjects.Count >= requiredTrashObjects)
            {
                Debug.Log("คันโยกพร้อมให้ใช้งานแล้ว!");
            }
        }
    }
}
