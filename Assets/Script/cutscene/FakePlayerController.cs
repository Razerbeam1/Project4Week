using UnityEngine;

public class FakePlayerController : MonoBehaviour
{
    public float moveSpeed = 3f; // ความเร็วในการเคลื่อนที่
    public Transform targetPos; // ตำแหน่งที่ Fake Player จะเดินไป
    public GameObject cutscenePanel; // อ้างอิงถึงพาแนลคัตซีน
    public GameObject gameSystemPanel; //อ้างอิงถึงพาแนล Game Play
    public GameObject UIgame_Play; // เปิด UI Gameplay
    
    private void Start()
    {
        if (targetPos != null)
        {
            StartMovingToTarget(); // เริ่มเดินไปยังตำแหน่งที่กำหนด
        }
        else
        {
            Debug.LogError("Target Position not set for FakePlayer!");
        }

        if (cutscenePanel == null)
        {
            Debug.LogWarning("Cutscene panel is not assigned!");
        }
    }

    void Update()
    {
        if (targetPos != null)
        {
            MoveTowardsTarget(); // เคลื่อนที่ไปยังเป้าหมาย
        }
    }

    void MoveTowardsTarget()
    {
        // คำนวณการเคลื่อนที่ไปยังเป้าหมาย
        transform.position = Vector3.MoveTowards(transform.position, targetPos.position, moveSpeed * Time.deltaTime);

        // ถ้าถึงตำแหน่งเป้าหมายแล้ว
        if (Vector3.Distance(transform.position, targetPos.position) < 0.1f)
        {
            OnReachTarget(); // เรียกฟังก์ชันเมื่อถึงเป้าหมาย
        }
    }

    void StartMovingToTarget()
    {
        // ทำให้ Fake Player เริ่มเคลื่อนที่เมื่อเกมเริ่ม
        Debug.Log("Fake Player starts moving to the target.");
    }

    void OnReachTarget()
    {
        Debug.Log("Fake Player has reached the target!");

        // ปิด Fake Player
        gameObject.SetActive(false);

        // ปิดพาแนลคัตซีน
        if (cutscenePanel != null)
        {
            cutscenePanel.SetActive(false);
            Debug.Log("Cutscene panel has been closed.");
        }
        
        // เปิดพาแนล Game System
        if (gameSystemPanel != null)
        {
            gameSystemPanel.SetActive(true);
            UIgame_Play.SetActive(true);
            Debug.Log("Game system panel has been opened.");
        }
    }
}