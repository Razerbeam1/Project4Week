using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CutsceneController : MonoBehaviour
{
    public Transform[] waypoints; // จุดหมายปลายทางในรูปแบบ Array
    public float moveSpeed = 2f; // ความเร็วในการเคลื่อนที่

    public GameObject dialogBox1; // กล่องข้อความไดอะล็อก 1
    public TMP_Text dialogText1; // ข้อความสำหรับไดอะล็อก 1
    public GameObject dialogBox2; // กล่องข้อความไดอะล็อก 2
    public TMP_Text dialogText2; // ข้อความสำหรับไดอะล็อก 2

    public Transform[] toys; // จุดตำแหน่งของเล่นที่ต้องไปเก็บ
    public GameObject[] rewardObjects; // Array สำหรับเก็บ Objs ที่จะปล่อยเมื่อเก็บของเล่นครบ
    public Transform rewardSpawnPoint; // ตำแหน่งที่ Obj จะถูกปล่อยออกมา

    private int currentWaypointIndex = 0; // ดัชนีของจุดหมายปัจจุบัน
    private int currentToyIndex = 0; // ดัชนีของของเล่นปัจจุบัน
    private bool isWalking = true; // ตัวแปรตรวจสอบว่ากำลังเดินอยู่หรือไม่
    private bool isCollectingToys = false; // ตัวแปรตรวจสอบว่ากำลังเก็บของเล่นหรือไม่

    void Start()
    {
        // ซ่อนกล่องข้อความไดอะล็อกในตอนเริ่มเกม
        HideDialog(dialogBox1);
        HideDialog(dialogBox2);
    }

    void Update()
    {
        if (isWalking && currentWaypointIndex < waypoints.Length)
        {
            MoveToWaypoint();
        }
        else if (isCollectingToys && currentToyIndex < toys.Length)
        {
            MoveToToy();
        }
    }

    void MoveToWaypoint()
    {
        Transform targetWaypoint = waypoints[currentWaypointIndex];
        transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.position, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetWaypoint.position) < 0.1f)
        {
            isWalking = false;
            HandleWaypointArrival();
        }
    }

    void HandleWaypointArrival()
    {
        if (currentWaypointIndex == 0)
        {
            ShowDialogSequence();
        }
    }

    void ShowDialogSequence()
    {
        StartCoroutine(DialogSequenceCoroutine());
    }

    private IEnumerator DialogSequenceCoroutine()
    {
        ShowDialog(dialogBox1, dialogText1, "มาถึงจุดหมายที่ 1");
        yield return new WaitForSeconds(3f);
        HideDialog(dialogBox1); // ซ่อนไดอะล็อก 1 หลังจากแสดงเสร็จ

        ShowDialog(dialogBox2, dialogText2, "เตรียมตัวเก็บของเล่น!");
        yield return new WaitForSeconds(3f);
        HideDialog(dialogBox2); // ซ่อนไดอะล็อก 2 หลังจากแสดงเสร็จ

        StartCollectingToys();
    }

    void MoveToToy()
    {
        Transform targetToy = toys[currentToyIndex];
        transform.position = Vector3.MoveTowards(transform.position, targetToy.position, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetToy.position) < 0.1f)
        {
            CollectToy(targetToy);
        }

        if (currentToyIndex >= toys.Length)
        {
            Debug.Log("เก็บของเล่นครบแล้ว");
            isCollectingToys = false;
            EndCutscene();
        }
    }

    void CollectToy(Transform toy)
    {
        Debug.Log("เก็บของเล่นชิ้นที่ " + (currentToyIndex + 1));
        Destroy(toy.gameObject);
        currentToyIndex++;
    }

    void ShowDialog(GameObject dialogBox, TMP_Text dialogText, string message)
    {
        if (dialogBox != null && dialogText != null)
        {
            dialogBox.SetActive(true);
            dialogText.text = message;
        }
    }

    void HideDialog(GameObject dialogBox)
    {
        if (dialogBox != null)
        {
            dialogBox.SetActive(false);
        }
    }

    void StartCollectingToys()
    {
        Debug.Log("เริ่มเก็บของเล่น");
        isCollectingToys = true;
    }

    void EndCutscene()
    {
        Debug.Log("ฉากคัตซีนจบแล้ว!");

        Debug.Log("EndCutscene() ถูกเรียกใช้งาน");
        // ปล่อย Obj ออกมาเมื่อเก็บของเล่นครบ
        SpawnRewardObjects();
    }

    void SpawnRewardObjects()
    {
        Debug.Log("SpawnRewardObjects() ถูกเรียกใช้งาน");
        if (rewardObjects.Length > 0)
        {
            foreach (GameObject reward in rewardObjects)
            {
                if (reward != null)
                {
                    reward.SetActive(true); // เปิดใช้งาน GameObject
                    Debug.Log("Reward: " + reward.name + " ถูกเปิดใช้งาน");
                }
                else
                {
                    Debug.LogError("มี Reward ที่เป็น null ใน rewardObjects Array");
                }
            }
        }
        else
        {
            Debug.LogError("RewardObjects Array ว่างเปล่าหรือไม่มีการตั้งค่าใน Inspector");
        }
    }


}
