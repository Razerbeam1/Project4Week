
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*public class LeverController : MonoBehaviour
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

    // ตัวแปรสำหรับเก็บจำนวน Object ที่ใส่ในกล่อง
    private List<GameObject> collectedObjects = new List<GameObject>(); 
    [SerializeField] int requiredTrashObjects = 2; // จำนวน Object ที่ต้องการในกล่อง

    // ตัวแปรสำหรับ Object Box ที่จะเปลี่ยน sprite เมื่อใส่ครบจำนวน
    [SerializeField] GameObject objectBox; // อ้างอิงถึง Object Box
    [SerializeField] Sprite completedSprite; // sprite เมื่อใส่ครบแล้ว

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
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) // ตรวจสอบว่าผู้เล่นออกจากการชน
        {
            isPlayerNear = false; // ตั้งค่าสถานะผู้เล่นไม่ใกล้
            Debug.Log("ผู้เล่นออกจากการชนคันโยก");
        }
    }

   
    // ฟังก์ชันที่ใช้สำหรับตรวจสอบเมื่อ Player ใส่ Object ลงใน Box
    public void AddObjectToBox(GameObject obj)
    {
        if (!collectedObjects.Contains(obj)) // ตรวจสอบว่า Object นี้ยังไม่ได้ถูกเพิ่ม
        {
            collectedObjects.Add(obj);
            Debug.Log("Object ถูกเพิ่มลงในกล่อง");

            // ลบ Object ที่ถูกเพิ่มออกจาก scene
            Destroy(obj); // ลบ Object ที่ใส่เข้าไป

            // ถ้าครบจำนวนแล้ว เปลี่ยน sprite ของ Object Box
            if (collectedObjects.Count >= requiredTrashObjects)
            {
                objectBox.GetComponent<SpriteRenderer>().sprite = completedSprite;
                Debug.Log("Object Box เต็มแล้ว");
            }
        }
    }

}
*/

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

    // ตัวแปรสำหรับเก็บจำนวน Object ที่ใส่ในกล่อง
    private List<GameObject> collectedObjects = new List<GameObject>(); 
    [SerializeField] int requiredTrashObjects = 2; // จำนวน Object ที่ต้องการในกล่อง

    // ตัวแปรสำหรับ Object Box ที่จะเปลี่ยน sprite เมื่อใส่ครบจำนวน
    [SerializeField] GameObject objectBox; // อ้างอิงถึง Object Box
    [SerializeField] Sprite completedSprite; // sprite เมื่อใส่ครบแล้ว

    private Collider2D objectBoxCollider; // ตัวแปรสำหรับเก็บ Collider ของ Object Box
    private Collider2D leverCollider; // ตัวแปรสำหรับเก็บ Collider ของคันโยก
    void Start()
    {
        originalPosition = transform.position; // บันทึกตำแหน่งเริ่มต้นของคันโยก
        dorFinalPosition = new Vector3(Dor.transform.position.x, Dor.transform.position.y - moveDistance, Dor.transform.position.z); // ตำแหน่งปลายทางของ Dor
        objectBoxCollider = objectBox.GetComponent<Collider2D>(); // หาค่า Collider2D ของ Object Box
        
        leverCollider = GetComponent<Collider2D>(); // หาค่า Collider ของคันโยก
        
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
            
            // ปิดคอลายเดอร์ของคันโยก
            if (leverCollider != null)
            {
                leverCollider.enabled = false; // ปิด Collider ของคันโยก
                Debug.Log("คอลายเดอร์ของคันโยกถูกปิดแล้ว");
            }
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
            
            // ซ่อนคันโยก (LeverController)
            gameObject.SetActive(false); // ซ่อนคันโยกหลังจากที่โยกเสร็จแล้ว
            Debug.Log("คันโยกถูกซ่อนไปแล้ว");
        }
    }


    // ฟังก์ชันที่ใช้สำหรับตรวจสอบเมื่อ Player ใส่ Object ลงใน Box
    public void AddObjectToBox(GameObject obj)
    {
        if (!collectedObjects.Contains(obj)) // ตรวจสอบว่า Object นี้ยังไม่ได้ถูกเพิ่ม
        {
            collectedObjects.Add(obj);
            Debug.Log("Object ถูกเพิ่มลงในกล่อง");

            // ลบ Object ที่ถูกเพิ่มออกจาก scene
            Destroy(obj); // ลบ Object ที่ใส่เข้าไป

            // ถ้าครบจำนวนแล้ว เปลี่ยน sprite ของ Object Box
            if (collectedObjects.Count >= requiredTrashObjects)
            {
                objectBox.GetComponent<SpriteRenderer>().sprite = completedSprite;
                Debug.Log("Object Box เต็มแล้ว");
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) // ตรวจสอบว่าชนกับผู้เล่น
        {
            isPlayerNear = true; // ตั้งค่าสถานะผู้เล่นใกล้
            Debug.Log("ผู้เล่นอยู่ใกล้คันโยก");
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
}

