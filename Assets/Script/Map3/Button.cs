using System;
using UnityEngine;

public class Button : MonoBehaviour
{
    private Rigidbody2D batteryRb;
    private bool nPress = true;

    private void Start()
    {
        // Find the object named "Battery" and get its Rigidbody2D component
        GameObject battery = GameObject.Find("Battery");
        if (battery != null)
        {
            batteryRb = battery.GetComponent<Rigidbody2D>();
        }
        else
        {
            Debug.LogError("Battery object not found. Make sure an object named 'Battery' exists in the scene.");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Trigger enter");
        // Check if the object entering the trigger has the tag "Player"
        if (collision.CompareTag("Player") && batteryRb != null && nPress)
        {
            batteryRb.gravityScale = 1;
            // Add 5 velocity going sideways (right direction)
            batteryRb.linearVelocity = new Vector2(-20, batteryRb.linearVelocity.y);
            Debug.Log("Added sideways velocity to Battery.");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        nPress = false;
    }
}
