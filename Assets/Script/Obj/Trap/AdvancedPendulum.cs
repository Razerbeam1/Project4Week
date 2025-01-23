using UnityEngine;

public class AdvancedPendulum : MonoBehaviour
{
    [SerializeField] public float swingForce; // แรงแกว่ง
    [SerializeField] public float maxAngle;  // มุมสูงสุด
    [SerializeField] public float floatSpeed; // ความเร็วในการลอย
    [SerializeField] public float floatRange; // ระยะการลอย

    private float currentAngle = 0f;
    private float direction = 1f; // ทิศทางแกว่ง
    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position; // บันทึกตำแหน่งเริ่มต้น
    }

    void Update()
    {
        // การแกว่ง
        currentAngle += direction * swingForce * Time.deltaTime;
        if (Mathf.Abs(currentAngle) >= maxAngle)
        {
            direction *= -1;
        }
        transform.localRotation = Quaternion.Euler(0f, 0f, currentAngle);

        // การลอย
        float newY = startPosition.y + Mathf.Sin(Time.time * floatSpeed) * floatRange;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}