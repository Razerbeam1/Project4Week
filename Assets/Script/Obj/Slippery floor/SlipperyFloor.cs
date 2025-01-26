using UnityEngine;

public class SlipperyFloor2D : MonoBehaviour
{
    [SerializeField] float speedBoostMultiplier = 1.5f; // ค่าคูณความเร็วสำหรับพื้นลื่น
    [SerializeField] float maxSpeed = 10f; // ความเร็วสูงสุดที่ผู้เล่นสามารถเดินได้
    [SerializeField] float frictionMultiplier = 0.95f; // ตัวคูณสำหรับลดความเร็ว (ช่วยให้หยุดช้าลงหลังจากออกจากพื้นที่)

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Rigidbody2D playerRigidbody = other.attachedRigidbody;

            if (playerRigidbody != null)
            {
                // ถ้า player มีความเร็วมากกว่า 0.1 ให้คูณความเร็วเพิ่มขึ้น
                Vector2 boostedVelocity = playerRigidbody.velocity.sqrMagnitude > 0.1f
                    ? playerRigidbody.velocity * speedBoostMultiplier
                    : playerRigidbody.velocity;

                // ตรวจสอบไม่ให้ความเร็วเกินขีดจำกัด
                if (boostedVelocity.magnitude > maxSpeed)
                {
                    boostedVelocity = boostedVelocity.normalized * maxSpeed;
                }

                playerRigidbody.velocity = boostedVelocity;

                // ทำให้มีการลดความเร็วอย่างช้าๆ ถ้าไม่อยู่บนพื้นที่ลื่นแล้ว
                if (!IsPlayerOnSlipperyFloor(other))
                {
                    playerRigidbody.velocity = playerRigidbody.velocity * frictionMultiplier;
                }
            }
        }
    }

    private bool IsPlayerOnSlipperyFloor(Collider2D other)
    {
        // เช็คว่า Player ยืนอยู่บนพื้นลื่นหรือไม่
        return other.CompareTag("Player");
    }
}