using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public bool grabbed; // Track whether the item is grabbed
    public Transform holdPoint; // The point where the item will be held
    public float throwForce = 5f; // Set a default throw force
    public LayerMask notGrabbed; // Mask for objects that cannot be grabbed

    private Collider objectCollider; // Reference to the collider of the grabbable object

    public AudioSource onItemPickup;
    public AudioSource onItemThrow;

    void Update()
    {
        // Check for the input to grab or throw the item
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (!grabbed && objectCollider != null) // If not grabbed and there's an object to grab
            {
                onItemPickup.Play();
                grabbed = true;

                // Disable the object's collider to avoid obstructing movement
                if (objectCollider != null)
                {
                    objectCollider.enabled = false; // Disable collider
                }

                // Optionally, disable the object's gravity while held
                Rigidbody rb = objectCollider.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.isKinematic = true; // Disable physics interactions
                }
            }
            else if (grabbed) // If currently holding the item, throw it
            {
                onItemThrow.Play();
                grabbed = false;

                // Re-enable the collider when the object is thrown
                if (objectCollider != null)
                {
                    objectCollider.enabled = true; // Re-enable collider
                }

                Rigidbody rb = objectCollider.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.isKinematic = false; // Re-enable physics
                    rb.velocity = transform.forward * throwForce; // Apply a force in the forward direction
                }

                objectCollider = null; // Clear the reference to the collider after throwing
            }
        }
    }

    void FixedUpdate()
    {
                // If currently holding the item, update its position
        if (grabbed && objectCollider != null)
        {
            objectCollider.transform.position = holdPoint.position;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Grabbable")) // Check if the object entering the trigger is grabbable
        {
            objectCollider = other; // Store the collider reference
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Grabbable")) // Check if the object exiting the trigger is grabbable
        {
            if (!grabbed)
            {
                objectCollider = null; // Clear the reference if not grabbed
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * 2f); // Adjust for visual feedback
    }
}
