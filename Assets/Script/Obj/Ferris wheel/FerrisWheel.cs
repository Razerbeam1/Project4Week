using UnityEngine;

public class FerrisWheel : MonoBehaviour
{
    [SerializeField] private float rotationSpeed; // ความเร็วในการหมุน (ค่าลบ = หมุนทวนเข็มนาฬิกา)
    [SerializeField] private Transform[] magnets; // ตัวแปรที่เก็บตำแหน่งของแม่เหล็กทั้งหมด (แม่เหล็ก 4 ลูก)

    private float radius; // รัศมีของวงกลมที่แม่เหล็กจะติดตาม

    void Start()
    {
        // คำนวณรัศมีจากตำแหน่งของแม่เหล็กในแนว X หรือ Y (ขึ้นอยู่กับการออกแบบ)
        if (magnets.Length > 0)
        {
            radius = Vector3.Distance(transform.position, magnets[0].position); // ใช้แม่เหล็กตัวแรกในการคำนวณ
        }
    }

    void Update()
    {
        // หมุนชิงช้าสวรรค์รอบแกน Z
        transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);

        // แบ่งแม่เหล็ก 4 ลูกให้มีตำแหน่งกระจายอยู่รอบวงกลม
        for (int i = 0; i < magnets.Length; i++)
        {
            // คำนวณมุมหมุนที่ต่างกันสำหรับแม่เหล็กแต่ละตัว
            float angle = transform.rotation.eulerAngles.z + (i * 90f); // 0°, 90°, 180°, 270°
            
            // คำนวณตำแหน่งใหม่ของแม่เหล็ก
            Vector3 newPosition = new Vector3(
                transform.position.x + radius * Mathf.Cos(Mathf.Deg2Rad * angle),
                transform.position.y + radius * Mathf.Sin(Mathf.Deg2Rad * angle),
                magnets[i].position.z // รักษาค่าของตำแหน่ง Z
            );

            // อัพเดตตำแหน่งแม่เหล็ก
            magnets[i].position = newPosition;
        }
    }
}