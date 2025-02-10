using System.Collections;
using UnityEngine;

public class FloorBreakable : MonoBehaviour
{
    [SerializeField] private float timeToBreak; // เวลาที่ต้องใช้ในการทำให้พื้นพัง
    [SerializeField] private float weightThreshold; // น้ำหนักที่ทำให้พื้นพังทันที
    [SerializeField] private Rigidbody2D floorRigidbody;
    [SerializeField] private float resetDelay; // เวลาหน่วงก่อนรีเซ็ตพื้นกลับตำแหน่งเดิม
    [SerializeField] private float replaceInterval = 1f; // เวลาระหว่างการเปลี่ยนแต่ละคู่
    [SerializeField] private GameObject[] replacementPairs; // คู่ของพื้นที่จะแทนที่กัน

    private int currentPairIndex = 0; // ติดตามคู่ที่กำลังใช้งานอยู่

    private float timePlayerOnFloor = 0f; // เวลาที่ผู้เล่นยืนบนพื้น
    private bool isPlayerOnFloor = false; // ตรวจสอบว่าผู้เล่นอยู่บนพื้นหรือไม่
    private bool isBroken = false; // ตรวจสอบว่าพื้นพังแล้วหรือยัง

    private Vector3 initialPosition; // ตำแหน่งเริ่มต้นของพื้น
    private Quaternion initialRotation; // การหมุนเริ่มต้นของพื้น

    public Rigidbody2D[] floorRbArray; // array สำหรับเก็บ Rigidbody2D ของแต่ละ obj

    private void Start()
    {
        initialPosition = transform.position;
        initialRotation = transform.rotation;

        // ตรวจสอบว่ามี floorRigidbody หรือไม่
        if (floorRigidbody == null)
        {
            floorRigidbody = GetComponent<Rigidbody2D>();
        }

        // ตั้งค่าให้พื้นเป็น Static
        if (floorRigidbody != null)
        {
            floorRigidbody.bodyType = RigidbodyType2D.Static;
        }

        // เก็บ Rigidbody2D ของทุกๆ obj ใน replacementPairs ลงใน array
        floorRbArray = new Rigidbody2D[replacementPairs.Length];
        for (int i = 0; i < replacementPairs.Length; i++)
        {
            GameObject obj = replacementPairs[i];
            if (obj != null)
            {
                Rigidbody2D[] rbs = obj.GetComponents<Rigidbody2D>();
                if (rbs != null && rbs.Length > 0)
                {
                    floorRbArray[i] = rbs[0]; // เก็บ Rigidbody2D ของแต่ละ obj ลงใน array
                }
            }
        }

        // ปิดการแสดงผลของทุก obj ใน replacementPairs
        foreach (GameObject obj in replacementPairs)
        {
            if (obj != null) obj.SetActive(false);
        }
    }

    private void Update()
    {
        if (isPlayerOnFloor && !isBroken)
        {
            timePlayerOnFloor += Time.deltaTime;
            if (timePlayerOnFloor >= timeToBreak)
            {
                BreakFloor();
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isPlayerOnFloor = true;
            Rigidbody2D playerRigidbody = collision.gameObject.GetComponent<Rigidbody2D>();
            if (playerRigidbody != null)
            {
                // ตรวจสอบน้ำหนักของผู้เล่น
                if (playerRigidbody.mass >= weightThreshold)
                {
                    BreakFloor();
                }
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isPlayerOnFloor = false;
            timePlayerOnFloor = 0f;
        }
    }

    private void BreakFloor()
    {
        if (isBroken) return;
        isBroken = true;
        Debug.Log("Floor has broken!");

        // เปลี่ยน `floorRigidbody` เป็น Dynamic เพื่อให้มันร่วงลง
        StartCoroutine(BreakAndReplaceFloor());
    }

    private IEnumerator BreakAndReplaceFloor()
    {
        // เปลี่ยน `floorRigidbody` เป็น Dynamic เพื่อให้มันร่วงลง
        if (floorRigidbody != null)
        {
            floorRigidbody.bodyType = RigidbodyType2D.Dynamic;
        }

        // รอให้พื้นร่วงไปก่อน (คุณสามารถปรับเวลานี้ได้)
        yield return new WaitForSeconds(0.5f);

        // เริ่มให้ obj คู่ๆ เกิดขึ้น เมื่อพื้นพังแล้ว
        while (currentPairIndex < replacementPairs.Length)
        {
            GameObject obj = replacementPairs[currentPairIndex];
            if (obj != null)
            {
                obj.SetActive(true);
                Rigidbody2D rb = floorRbArray[currentPairIndex]; // ใช้ Rigidbody2D จาก array
                if (rb != null)
                {
                    rb.bodyType = RigidbodyType2D.Dynamic; // เปลี่ยนเป็น Dynamic เพื่อให้ร่วงลง
                }
            }
            currentPairIndex++;
            yield return new WaitForSeconds(replaceInterval); // รอให้แต่ละ obj เกิดขึ้นตามช่วงเวลา
        }

        // รีเซ็ตพื้นกลับไปที่ตำแหน่งเดิม
        yield return new WaitForSeconds(1f);
        StartCoroutine(ResetFloor());
    }

    private IEnumerator ResetFloor()
    {
        yield return new WaitForSeconds(resetDelay);

        transform.position = initialPosition;
        transform.rotation = initialRotation;

        if (floorRigidbody != null)
        {
            floorRigidbody.bodyType = RigidbodyType2D.Static;
            floorRigidbody.velocity = Vector2.zero;
            floorRigidbody.angularVelocity = 0f;
        }

        currentPairIndex = 0;
        isBroken = false;
        timePlayerOnFloor = 0f;

        foreach (GameObject obj in replacementPairs)
        {
            if (obj != null) obj.SetActive(false);
        }

        gameObject.SetActive(true);
        Debug.Log("Floor has been reset to its original position.");
    }
}


