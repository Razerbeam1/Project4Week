using UnityEngine;

public class ObjectBoxController : MonoBehaviour
{
    [SerializeField] LeverController leverController; // อ้างอิงถึง LeverController

    void OnTriggerEnter2D(Collider2D other)
    {
        // ตรวจสอบว่า Object ที่ชนมี Tag เป็น "gear , iron"
        if (other.CompareTag("gear"))
        {
            // เรียกฟังก์ชัน AddObjectToBox เพื่อเพิ่ม Object ลงในกล่อง
            if (leverController != null)
            {
                leverController.AddObjectToBox(other.gameObject);
            }
        }
        if (other.CompareTag("iron"))
        {
            // เรียกฟังก์ชัน AddObjectToBox เพื่อเพิ่ม Object ลงในกล่อง
            if (leverController != null)
            {
                leverController.AddObjectToBox(other.gameObject);
            }
        }
    }
}