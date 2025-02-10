/*
using UnityEngine;

public class FerrisWheel3 : MonoBehaviour
{
    private Transform player;
    private Rigidbody2D playerRb;
    private bool playerOnPlatform = false;

    void FixedUpdate()
    {
        if (playerOnPlatform && player != null)
        {
            // Move the player with the platform
            player.position += (Vector3)(GetComponent<Rigidbody2D>().linearVelocity * Time.fixedDeltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.transform;
            playerRb = player.GetComponent<Rigidbody2D>();
            playerOnPlatform = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerOnPlatform = false;
            player = null;
            playerRb = null;
        }
    }
}
*/



using UnityEngine;

public class FerrisWheel3 : MonoBehaviour
{
    private Transform player;
    private Rigidbody2D playerRb;
    private bool playerOnPlatform = false;
    private FixedJoint2D fixedJoint;

    void FixedUpdate()
    {
        if (playerOnPlatform && player != null)
        {
            // Player จะเคลื่อนที่ไปพร้อมกับ Ferris Wheel เนื่องจาก FixedJoint2D
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.transform;
            playerRb = player.GetComponent<Rigidbody2D>();

            // ติดตั้ง FixedJoint2D เพื่อให้ Player ติดไปกับ Ferris Wheel
            fixedJoint = player.gameObject.AddComponent<FixedJoint2D>();
            fixedJoint.connectedBody = GetComponent<Rigidbody2D>(); // เชื่อมต่อกับ Rigidbody2D ของ Ferris Wheel

            playerOnPlatform = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // ถอด FixedJoint2D เมื่อ Player ออกจาก Ferris Wheel
            if (fixedJoint != null)
            {
                Destroy(fixedJoint);
            }

            playerOnPlatform = false;
            player = null;
            playerRb = null;
        }
    }
}

