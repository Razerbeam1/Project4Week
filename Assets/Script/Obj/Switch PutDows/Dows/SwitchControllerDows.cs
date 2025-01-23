using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchControllerDows : MonoBehaviour
{
    [SerializeField] GameObject door; // อ้างอิงถึงประตูที่จะเลื่อนลง
    [SerializeField] float moveDistance; // ระยะทางที่ประตูจะเลื่อน
    [SerializeField] float moveSpeed; // ความเร็วในการเลื่อนประตู

    private int weightOnSwitch = 0; // นับจำนวนวัตถุที่มีน้ำหนักบนสวิตช์
    private bool isPressed = false; // สถานะของ switch
    private Vector3 doorOriginalPosition; // ตำแหน่งเริ่มต้นของประตู
    private Vector3 doorLoweredPosition; // ตำแหน่งที่ประตูจะเลื่อนลง

    private bool isPlayerNear = false; // ตรวจสอบว่าผู้เล่นอยู่ใกล้หรือไม่

    void Start()
    {
        // บันทึกตำแหน่งเริ่มต้นของประตู
        doorOriginalPosition = door.transform.position;

        // คำนวณตำแหน่งเลื่อนลง
        doorLoweredPosition = new Vector3(door.transform.position.x, door.transform.position.y - moveDistance, door.transform.position.z);
    }

    void Update()
    {
        // ถ้ามีวัตถุกดทับสวิตช์ ให้ประตูเลื่อนลง
        if (isPressed)
        {
            door.transform.position = Vector3.MoveTowards(door.transform.position, doorLoweredPosition, moveSpeed * Time.deltaTime);
        }
        // ถ้าไม่มีวัตถุกดทับ ให้ประตูเลื่อนกลับขึ้นไปที่ตำแหน่งเดิม
        else
        {
            door.transform.position = Vector3.MoveTowards(door.transform.position, doorOriginalPosition, moveSpeed * Time.deltaTime);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("TrashObject") || collision.CompareTag("Player"))
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
        if (collision.CompareTag("TrashObject") || collision.CompareTag("Player"))
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
