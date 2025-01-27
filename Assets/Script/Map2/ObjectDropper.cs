using UnityEngine;

public class ObjectDropper : MonoBehaviour
{
    public GameObject objectPrefab; // พรีแฟบของวัตถุที่ต้องหล่น
    public Transform[] spawnPoints; // จุดปล่อยวัตถุ (Empty GameObjects)
    public float dropInterval = 5f; // ระยะเวลาที่วัตถุจะหล่น (วินาที)
    public float fallSpeed = 5f; // ความเร็วที่วัตถุหล่นลงด้านล่าง
    public float destroyDelay = 10f; // ระยะเวลาที่วัตถุจะถูกทำลายหลังจากหล่น

    private void Start()
    {
        // เรียกใช้งานการปล่อยวัตถุทุกๆ dropInterval วินาที
        InvokeRepeating(nameof(SpawnObject), 0f, dropInterval);
    }

    private void SpawnObject()
    {
        // เลือกตำแหน่งปล่อยวัตถุแบบสุ่มจาก spawnPoints
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        // สร้างวัตถุใหม่ที่ตำแหน่งปล่อย
        GameObject obj = Instantiate(objectPrefab, spawnPoint.position, Quaternion.identity);

        // เพิ่ม Rigidbody2D เพื่อให้วัตถุหล่นลง
        Rigidbody2D rb = obj.AddComponent<Rigidbody2D>();
        rb.gravityScale = 0; // ปิดแรงโน้มถ่วง
        rb.linearVelocity = Vector2.down * fallSpeed; // ทำให้หล่นในแนวดิ่งด้วยความเร็วที่กำหนด

        // ทำลายวัตถุหลังจากเวลาที่กำหนด
        Destroy(obj, destroyDelay);
    }
}
