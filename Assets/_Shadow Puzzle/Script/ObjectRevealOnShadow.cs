using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRevealOnShadow : MonoBehaviour
{
    // Reference to the object to be activated
    public GameObject targetObject;

    private void OnTriggerEnter(Collider other)
    {
        // Check if the collider entering is the shadow collider
        if (other.CompareTag("Traps") && targetObject != null)
        {
            RevealObject();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Check if the collider exiting is the shadow collider
        if (other.CompareTag("Traps") && targetObject != null)
        {
            HideObject();
        }
    }

    private void RevealObject()
    {
        targetObject.SetActive(true);
        Debug.Log($"{targetObject.name} activated.");
    }

    private void HideObject()
    {
        targetObject.SetActive(false);
        Debug.Log($"{targetObject.name} deactivated.");
    }
}
