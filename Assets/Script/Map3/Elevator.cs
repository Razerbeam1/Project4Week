
using UnityEngine;
using System.Collections;

public class Elevator : MonoBehaviour
{
    public float moveDistance = 2f; // How far the object moves up and down
    public float speed = 2f; // Speed of movement
    public float waitTime = 1f; // Time to wait at the top and bottom
    public BatteryFuse batteryFuse; // Reference to the BatteryFuse script

    private Vector2 startPos;
    private bool movingUp = true;
    private bool isWaiting = false;
    private bool isActivated = false; // Ensures movement starts only when charged

    void Start()
    {
        startPos = transform.position; // Store the starting position
    }

    void Update()
    {
        // Check if battery is charged
        if (batteryFuse != null && batteryFuse.charged && !isActivated)
        {
            isActivated = true; // Activate movement
        }

        // Move only if activated and not waiting
        if (isActivated && !isWaiting)
        {
            MoveObject();
        }
    }

    void MoveObject()
    {
        // Determine target position
        float targetY = movingUp ? startPos.y + moveDistance : startPos.y - moveDistance;

        // Move the object
        transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x, targetY), speed * Time.deltaTime);

        // Check if the object has reached the target position
        if (Mathf.Abs(transform.position.y - targetY) < 0.01f)
        {
            StartCoroutine(WaitBeforeMoving());
        }
    }

    IEnumerator WaitBeforeMoving()
    {
        isWaiting = true;
        yield return new WaitForSeconds(waitTime); // Wait before switching direction
        movingUp = !movingUp; // Reverse direction
        isWaiting = false;
    }

}
