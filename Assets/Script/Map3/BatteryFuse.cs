/*
using UnityEngine;

public class BatteryFuse : MonoBehaviour
{
    public bool charged = false;
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Loop through all child objects of the colliding object
        foreach (Transform child in other.transform)
        {
            // Check if the child object has the name "Battery"
            if (child.name == "Battery")
            {
                //turn on elevator
                charged = true;
                
                // Detach the battery from its parent
                child.SetParent(null);

                // Position the battery at the trigger's location
                child.position = transform.position;

                // Reset rotation to (0,0)
                child.rotation = Quaternion.identity;

                // Ensure the battery has a Rigidbody2D
                Rigidbody2D rb = child.GetComponent<Rigidbody2D>();
                if (rb == null)
                {
                    rb = child.gameObject.AddComponent<Rigidbody2D>(); // Add Rigidbody2D if missing
                }

                // Freeze X and Y position, allow only rotation
                rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;

                // Disable collider to prevent further interactions
                Collider2D batteryCollider = child.GetComponent<Collider2D>();
                if (batteryCollider != null)
                {
                    batteryCollider.enabled = false;
                }

                // Exit the loop after processing the first "Battery" found (optional)
                break;
            }
        }
    }
}
*/


using UnityEngine;

public class BatteryFuse : MonoBehaviour
{
    public bool charged = false; // ตัวแปรตรวจสอบว่า Battery ถูกติดตั้งสำเร็จหรือยัง
    private GameObject batteryInRange = null; // เก็บ Battery ที่อยู่ในระยะของตัว Trigger

    private void OnTriggerEnter2D(Collider2D other)
    {
        // ตรวจสอบว่ามีวัตถุที่ชื่อ "Battery" เข้ามาในระยะหรือไม่
        foreach (Transform child in other.transform)
        {
            if (child.name == "Battery")
            {
                batteryInRange = child.gameObject; // เก็บ Battery ที่อยู่ในระยะ
                Debug.Log("Battery อยู่ในระยะ สามารถกด E เพื่อใส่ได้");
                break;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // ตรวจสอบว่า Battery ออกจากระยะหรือไม่
        if (batteryInRange != null && other.gameObject == batteryInRange)
        {
            batteryInRange = null; // ล้างค่าเมื่อ Battery ออกจากระยะ
            Debug.Log("Battery ออกจากระยะแล้ว");
        }
    }

    private void Update()
    {
        // เช็คว่าผู้เล่นกดปุ่ม E และมี Battery ในระยะหรือไม่
        if (batteryInRange != null && Input.GetKeyDown(KeyCode.E))
        {
            InsertBattery(batteryInRange);
        }
    }

    private void InsertBattery(GameObject battery)
    {
        // ตั้งค่าว่า Battery ถูกชาร์จแล้ว
        charged = true;

        // ถอด Battery ออกจาก Parent เดิม (เช่น Player หรือ Object อื่น)
        battery.transform.SetParent(null);

        // ย้ายตำแหน่งของ Battery ไปอยู่ตำแหน่งของตัว Trigger
        battery.transform.position = transform.position;

        // รีเซ็ตการหมุนของ Battery ให้กลับมาเป็นค่าเริ่มต้น (0,0)
        battery.transform.rotation = Quaternion.identity;

        // ดึง Rigidbody2D ของ Battery
        Rigidbody2D rb = battery.GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            // ถ้า Battery ไม่มี Rigidbody2D ให้เพิ่มเข้าไป
            rb = battery.AddComponent<Rigidbody2D>();
        }

        // ล็อกตำแหน่ง X และ Y ของ Battery (ให้หมุนได้อย่างเดียว)
        rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;

        // ปิด Collider ของ Battery เพื่อป้องกันการชนอีกครั้ง
        Collider2D batteryCollider = battery.GetComponent<Collider2D>();
        if (batteryCollider != null)
        {
            batteryCollider.enabled = false;
        }

        // ล้างค่าตัวแปร batteryInRange
        batteryInRange = null;

        Debug.Log("ใส่ Battery แล้ว!");
    }
}
