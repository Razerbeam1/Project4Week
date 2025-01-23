using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    // กำหนดตัวแปรสำหรับบันทึกตำแหน่งของเช็คพอยต์
    public static Vector3[] checkpointPositions = new Vector3[3]; // มี 3 เช็คพอยต์
    public static bool[] checkpointReached = new bool[3]; // สถานะว่าแต่ละเช็คพอยต์ถูกผ่านหรือยัง

    public int checkpointIndex = 0; // ตัวบ่งชี้ว่าเช็คพอยต์ไหนถูกผ่าน

    private void Start()
    {
        // กำหนดตำแหน่งของเช็คพอยต์ในโลก
        checkpointPositions[0] = new Vector3(0, 0, 0); // เช็คพอยต์ที่ 1
        checkpointPositions[1] = new Vector3(10, 0, 0); // เช็คพอยต์ที่ 2
        checkpointPositions[2] = new Vector3(20, 0, 0); // เช็คพอยต์ที่ 3

        for (int i = 0; i < checkpointReached.Length; i++)
        {
            checkpointReached[i] = false; // เริ่มต้นเช็คพอยต์ยังไม่ถูกผ่าน
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // บันทึกตำแหน่งของเช็คพอยต์ที่ผู้เล่นผ่าน
            checkpointPositions[checkpointIndex] = transform.position;
            checkpointReached[checkpointIndex] = true;

            // เพิ่มหมายเลขเช็คพอยต์ถัดไป
            checkpointIndex = Mathf.Min(checkpointIndex + 1, checkpointPositions.Length - 1);

            Debug.Log("Checkpoint " + (checkpointIndex + 1) + " Reached!");
        }
    }
}