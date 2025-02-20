using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowMeshOnEnter : MonoBehaviour
{
    [SerializeField] private GameObject targetObject;  // The object whose MeshRenderer you want to control

    private MeshRenderer targetRenderer;

    private void Start()
    {
        if (targetObject != null)
        {
            // Get the MeshRenderer component of the target object
            targetRenderer = targetObject.GetComponent<MeshRenderer>();
            if (targetRenderer == null)
            {
                Debug.LogWarning("No MeshRenderer found on the target object: " + targetObject.name);
            }
            else
            {
                // Initially set the MeshRenderer to inactive (hidden)
                targetRenderer.enabled = false;
            }
        }
        else
        {
            Debug.LogError("Target object is not assigned in " + gameObject.name);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the collider entering is the player or any other specified tag
        if (other.CompareTag("Player") && targetRenderer != null)
        {
            // Activate the MeshRenderer
            targetRenderer.enabled = true;
            Debug.Log("MeshRenderer enabled for: " + targetObject.name);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Optionally disable the MeshRenderer when the player exits the trigger
        if (other.CompareTag("Player") && targetRenderer != null)
        {
            // Deactivate the MeshRenderer
            targetRenderer.enabled = false;
            Debug.Log("MeshRenderer disabled for: " + targetObject.name);
        }
    }
}
