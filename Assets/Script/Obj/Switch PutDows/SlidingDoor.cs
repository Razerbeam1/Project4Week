using UnityEngine;

public class SlidingDoor : MonoBehaviour
{
    [SerializeField] GameObject door; // อ้างอิงถึงประตูที่จะเลื่อน
    [SerializeField] float moveDistance; // ระยะทางที่ประตูจะเลื่อน
    [SerializeField] float moveSpeed; // ความเร็วในการเลื่อนประตู

    private int weightOnSwitch = 0; // นับจำนวนวัตถุที่มีน้ำหนักบนสวิตช์
    private bool isPressed = false; // สถานะของ switch
    private Vector3 doorOriginalPosition; // ตำแหน่งเริ่มต้นของประตู
    private Vector3 doorSlidePosition; // ตำแหน่งที่ประตูจะเลื่อนไปทางด้านซ้าย

    void Start()
    {
        // บันทึกตำแหน่งเริ่มต้นของประตู
        doorOriginalPosition = door.transform.position;

        // คำนวณตำแหน่งเลื่อนไปทางซ้าย (ลดค่า X)
        doorSlidePosition = new Vector3(door.transform.position.x - moveDistance, door.transform.position.y, door.transform.position.z);
    }

    void Update()
    {
        // ถ้ามีวัตถุกดทับสวิตช์ ให้ประตูเลื่อนไปทางซ้าย
        if (isPressed)
        {
            door.transform.position = Vector3.MoveTowards(door.transform.position, doorSlidePosition, moveSpeed * Time.deltaTime);
        }
        // ถ้าไม่มีวัตถุกดทับ ให้ประตูเลื่อนกลับไปที่ตำแหน่งเดิม
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
    }

    void PressSwitch()
    {
        // สวิตช์ถูกกด ทำงานตามที่ต้องการ
        Debug.Log("Switch Pressed");
    }

    void ReleaseSwitch()
    {
        // สวิตช์ถูกปล่อยคืนสภาพเดิม
        Debug.Log("Switch Released");
    }
}
