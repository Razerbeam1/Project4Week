using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchControllerX : MonoBehaviour
{
    [SerializeField] GameObject door; // อ้างอิงถึงประตูที่จะเลื่อนขึ้นลง
    [SerializeField] float moveDistance; // ระยะทางที่ประตูจะเลื่อน
    [SerializeField] float moveSpeed; // ความเร็วในการเลื่อนประตู
    
    private int weightOnSwitch = 0; // นับจำนวนวัตถุที่มีน้ำหนักบนสวิตช์
    private bool isPressed = false; // สถานะของ switch
    private Vector3 doorOriginalPosition; // ตำแหน่งเริ่มต้นของประตู
    private Vector3 doorRaisedPosition; // ตำแหน่งที่ประตูจะเลื่อนขึ้น
    
    private float debounceTime = 0.1f;
    private float lastTriggerTime = 0f;
    
    private bool isPlayerNear = false; // ตรวจสอบว่าผู้เล่นอยู่ใกล้หรือไม่

    /*private AudioManager audioManager; // เสียงในเกม
    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>(); // เสียงในเกม
       
    }*/
    
    void Start()
    {
        // บันทึกตำแหน่งเริ่มต้นของประตู
        doorOriginalPosition = door.transform.position;
        // เลื่อนขึ้น
        doorRaisedPosition = new Vector3(door.transform.position.x + moveDistance, door.transform.position.y, door.transform.position.z);
    }

    void Update()
    {
        // ถ้ามีวัตถุกดทับสวิตช์ ให้ประตูเลื่อนขึ้น
        if (isPressed)
        {
            door.transform.position = Vector3.MoveTowards(door.transform.position, doorRaisedPosition, moveSpeed * Time.deltaTime);
            //audioManager.PlaySFX(audioManager.SwitchPutDows); // เสียงSFX Get Hp
        }
        // ถ้าไม่มีวัตถุกดทับ ให้ประตูเลื่อนกลับลงไปที่ตำแหน่งเดิม
        else
        {
            door.transform.position = Vector3.MoveTowards(door.transform.position, doorOriginalPosition, moveSpeed * Time.deltaTime);
        }
    }
    #region <อันเดิม>
    /*void OnCollisionEnter2D(Collision2D collision)
    {
        // ตรวจสอบวัตถุที่มีแท็ก "Box", "Small Box", "Big Box", หรือ "Player"
        if (collision.gameObject.CompareTag("Box") || 
            collision.gameObject.CompareTag("Small Box") || 
            collision.gameObject.CompareTag("Big Box") || 
            collision.gameObject.CompareTag("Player"))
        {
            weightOnSwitch++; // เพิ่มจำนวนวัตถุที่กดทับสวิตช์
            if (weightOnSwitch == 1)
            {
                isPressed = true; // ตั้งค่าสถานะว่าสวิตช์ถูกกด
                PressSwitch(); // เรียกฟังก์ชันการทำงานของสวิตช์
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        // เมื่อวัตถุที่มีแท็ก "Box", "Small Box", "Big Box", หรือ "Player" ออกจากการชน
        if (collision.gameObject.CompareTag("Box") || 
            collision.gameObject.CompareTag("Small Box") || 
            collision.gameObject.CompareTag("Big Box") || 
            collision.gameObject.CompareTag("Player"))
        {
            weightOnSwitch--; // ลดจำนวนวัตถุที่กดทับสวิตช์
            if (weightOnSwitch <= 0) // ถ้าไม่มีวัตถุกดทับ
            {
                weightOnSwitch = 0; // ป้องกัน weightOnSwitch ติดลบ
                isPressed = false; // ตั้งค่าสถานะว่าสวิตช์ถูกปล่อย
                ReleaseSwitch(); // เรียกฟังก์ชันการทำงานเมื่อสวิตช์ถูกปล่อย
            }
        }
    }*/
    #endregion
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Box") || 
            collision.CompareTag("Small Box") || 
            collision.CompareTag("Big Box") || 
            collision.CompareTag("Player"))
        {
            weightOnSwitch++;
            Debug.Log("Object entered the switch, weightOnSwitch: " + weightOnSwitch);
            if (weightOnSwitch == 1 && !isPressed)
            {
                isPressed = true;
                PressSwitch();
            }
        }
        
        if (collision.gameObject.CompareTag("Player")) // ตรวจสอบว่าชนกับผู้เล่น
        {
            isPlayerNear = true; // ตั้งค่าสถานะผู้เล่นใกล้
            Debug.Log("ผู้เล่นอยู่ใกล้Switch");
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Box") || 
            collision.CompareTag("Small Box") || 
            collision.CompareTag("Big Box") || 
            collision.CompareTag("Player"))
        {
            weightOnSwitch--;
            Debug.Log("Object exited the switch, weightOnSwitch: " + weightOnSwitch);
            if (weightOnSwitch <= 0)
            {
                weightOnSwitch = 0;
                isPressed = false;
                ReleaseSwitch();
            }
        }
        
        if (collision.gameObject.CompareTag("Player")) // ตรวจสอบว่าผู้เล่นออกจากการชน
        {
            isPlayerNear = false; // ตั้งค่าสถานะผู้เล่นไม่ใกล้
            Debug.Log("ผู้เล่นออกจากSwitch");
        }
    }

    void PressSwitch()
    {
        // สวิตช์ถูกกด ทำงานตามที่ต้องการ
        Vector3 newPosition = transform.position;
        newPosition.y -= 0.1f; // ปรับตำแหน่ง Y ลงเมื่อสวิตช์ถูกกด
        transform.position = newPosition;
    }

    void ReleaseSwitch()
    {
        // สวิตช์ถูกปล่อยคืนสภาพเดิม
        Vector3 newPosition = transform.position;
        newPosition.y += 0.1f; // ปรับตำแหน่ง Y กลับเมื่อสวิตช์ถูกปล่อย
        transform.position = newPosition;
    }
}
