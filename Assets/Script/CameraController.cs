using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform player; // ตัวละครที่กล้องจะติดตาม
    [SerializeField] private Vector3 offset; // ระยะห่างระหว่างกล้องกับผู้เล่น
    [SerializeField] private float smoothSpeed; // ความเร็วในการปรับตำแหน่งกล้อง (ความนุ่มนวล)

    private void LateUpdate()
    {
        FollowPlayer();
    }

    private void FollowPlayer()
    {
        // คำนวณตำแหน่งกล้องเป้าหมายตามตำแหน่งผู้เล่น
        Vector3 targetPosition = player.position + offset;

        // ใช้ Smooth Damping เพื่อปรับตำแหน่งกล้องให้นุ่มนวล
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, targetPosition, smoothSpeed);

        // อัปเดตตำแหน่งกล้อง
        transform.position = smoothedPosition;
    }
}