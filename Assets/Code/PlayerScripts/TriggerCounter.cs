using System.Collections.Generic;
using UnityEngine;

public class TriggerCounter : MonoBehaviour
{
    private int objectCount = 0;
    // Property to get the current number of intersecting objects
    public int ObjectCount
    {
        get { return objectCount; }
    }

    // Public LayerMask to specify which layers to detect
    public LayerMask groundLayerMask;

    // List to keep track of rigidbodies inside the trigger
    private List<Rigidbody> rigidbodiesInTrigger = new List<Rigidbody>();

    // Public Vector3 to store the average linear velocity
    public Vector3 averageLinearVelocity = Vector3.zero;
    void FixedUpdate()
    {
        UpdateAverageVelocity();
    }

    // Called when an object enters the trigger collider
    private void OnTriggerEnter(Collider other)
    {
        // Check if the other object is on the ground layer
        if ((groundLayerMask.value & (1 << other.gameObject.layer)) != 0)
        {
            // Increment the counter when an object enters
            objectCount++;

            // Check if the object has a Rigidbody and add it to the list
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null && !rigidbodiesInTrigger.Contains(rb))
            {
                rigidbodiesInTrigger.Add(rb);
                UpdateAverageVelocity();
            }
        }
    }

    // Called when an object exits the trigger collider
    private void OnTriggerExit(Collider other)
    {
        // Check if the other object is on the ground layer
        if ((groundLayerMask.value & (1 << other.gameObject.layer)) != 0)
        {
            // Decrement the counter when an object exits
            objectCount--;

            // Check if the object has a Rigidbody and remove it from the list
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null && rigidbodiesInTrigger.Contains(rb))
            {
                rigidbodiesInTrigger.Remove(rb);
                UpdateAverageVelocity();
            }
        }
    }

    // Method to update the average linear velocity of all rigidbodies in the trigger
    private void UpdateAverageVelocity()
    {
        if (rigidbodiesInTrigger.Count == 0)
        {
            averageLinearVelocity = Vector3.zero;
            return;
        }

        Vector3 totalVelocity = Vector3.zero;
        foreach (Rigidbody rb in rigidbodiesInTrigger)
        {
            totalVelocity += rb.linearVelocity;
        }
        averageLinearVelocity = totalVelocity / rigidbodiesInTrigger.Count;
    }
}
