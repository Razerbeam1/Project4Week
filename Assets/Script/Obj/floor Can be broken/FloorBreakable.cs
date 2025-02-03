
/*using System.Collections;
using UnityEngine;

public class FloorBreakable : MonoBehaviour
{
    [SerializeField] private float timeToBreak; // เวลาที่ต้องใช้ในการทำให้พื้นพัง
    [SerializeField] private float weightThreshold; // น้ำหนักที่ทำให้พื้นพังทันที
    [SerializeField] private Rigidbody2D floorRigidbody; // Rigidbody ของพื้น
    [SerializeField] private float resetDelay; // เวลาที่ใช้รอก่อนรีเซ็ตกลับตำแหน่งเดิม

    private float timePlayerOnFloor = 0f; // เวลาที่ผู้เล่นยืนบนพื้น
    private bool isPlayerOnFloor = false; // ตรวจสอบว่าผู้เล่นอยู่บนพื้นหรือไม่
    private bool isBroken = false; // ตรวจสอบว่าพื้นพังแล้วหรือยัง

    private Vector3 initialPosition; // ตำแหน่งเริ่มต้นของพื้น
    private Quaternion initialRotation; // การหมุนเริ่มต้นของพื้น

    private void Start()
    {
        // เก็บตำแหน่งและการหมุนเริ่มต้น
        initialPosition = transform.position;
        initialRotation = transform.rotation;

        if (floorRigidbody == null)
        {
            floorRigidbody = GetComponent<Rigidbody2D>();
        }

        // ล็อคพื้นในตำแหน่งเดิมจนกว่าจะพัง
        if (floorRigidbody != null)
        {
            floorRigidbody.bodyType = RigidbodyType2D.Static;
        }
    }

    private void Update()
    {
        if (isPlayerOnFloor && !isBroken)
        {
            // เพิ่มเวลาที่ผู้เล่นอยู่บนพื้น
            timePlayerOnFloor += Time.deltaTime;

            // ถ้าเวลาที่ผู้เล่นอยู่บนพื้นเกินกำหนด ให้พื้นพัง
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

            // ตรวจสอบน้ำหนักของผู้เล่น
            Rigidbody2D playerRigidbody = collision.gameObject.GetComponent<Rigidbody2D>();
            if (playerRigidbody != null && playerRigidbody.mass >= weightThreshold)
            {
                BreakFloor(); // พื้นพังทันทีถ้าผู้เล่นน้ำหนักเกิน
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isPlayerOnFloor = false;
            timePlayerOnFloor = 0f; // รีเซ็ตเวลาที่ผู้เล่นยืนบนพื้น
        }
    }

    private void BreakFloor()
    {
        if (isBroken) return;

        isBroken = true; // ตั้งค่าสถานะว่าพื้นพังแล้ว
        Debug.Log("Floor has broken!");

        // ปลดล็อก Rigidbody ให้พื้นตกลง
        if (floorRigidbody != null)
        {
            floorRigidbody.bodyType = RigidbodyType2D.Dynamic;
        }

        // รอรีเซ็ตพื้นกลับตำแหน่งเดิม
        StartCoroutine(ResetFloor());
    }

    private IEnumerator ResetFloor()
    {
        yield return new WaitForSeconds(resetDelay); // รอให้พื้นตกลงไปก่อน

        // รีเซ็ตตำแหน่งและการหมุน
        transform.position = initialPosition;
        transform.rotation = initialRotation;

        // รีเซ็ต Rigidbody กลับไปเป็น Static
        if (floorRigidbody != null)
        {
            floorRigidbody.bodyType = RigidbodyType2D.Static;
            floorRigidbody.velocity = Vector2.zero; // รีเซ็ตความเร็วของ Rigidbody
            floorRigidbody.angularVelocity = 0f;   // รีเซ็ตการหมุนของ Rigidbody
        }

        // รีเซ็ตสถานะพื้น
        isBroken = false;
        timePlayerOnFloor = 0f;

        Debug.Log("Floor has been reset to its original position.");
    }
}*/


/*using System.Collections;
using UnityEngine;

public class FloorBreakable : MonoBehaviour
{
    [SerializeField] private float timeToBreak;
    [SerializeField] private float weightThreshold;
    [SerializeField] private Rigidbody2D floorRigidbody;
    [SerializeField] private float resetDelay;
    [SerializeField] private float replaceInterval = 1f; // เวลาระหว่างการเปลี่ยนแต่ละคู่

    [SerializeField] private GameObject[] replacementPairs;
    private int currentPairIndex = 0;

    private float timePlayerOnFloor = 0f;
    private bool isPlayerOnFloor = false;
    private bool isBroken = false;

    private Vector3 initialPosition;
    private Quaternion initialRotation;

    private void Start()
    {
        initialPosition = transform.position;
        initialRotation = transform.rotation;

        if (floorRigidbody == null)
        {
            floorRigidbody = GetComponent<Rigidbody2D>();
        }

        if (floorRigidbody != null)
        {
            floorRigidbody.bodyType = RigidbodyType2D.Static;
        }

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
            if (playerRigidbody != null && playerRigidbody.mass >= weightThreshold)
            {
                BreakFloor();
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

        StartCoroutine(ReplaceFloor());
    }

    private IEnumerator ReplaceFloor()
    {
        while (currentPairIndex < replacementPairs.Length)
        {
            for (int i = 0; i < 2 && currentPairIndex < replacementPairs.Length; i++)
            {
                GameObject obj = replacementPairs[currentPairIndex];
                if (obj != null)
                {
                    obj.SetActive(true);
                    Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
                    if (rb != null)
                    {
                        rb.bodyType = RigidbodyType2D.Static;
                    }
                }
                currentPairIndex++;
            }
            yield return new WaitForSeconds(replaceInterval);
        }

        yield return new WaitForSeconds(1f);
        if (floorRigidbody != null)
        {
            floorRigidbody.bodyType = RigidbodyType2D.Dynamic;
        }
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
}*/

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


