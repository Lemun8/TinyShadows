using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MimicTrigger : MonoBehaviour
{
    [SerializeField] private GameObject prefabToInstantiate; // The prefab to instantiate
    [SerializeField] private GameObject targetToDestroy; // The GameObject to destroy
    [SerializeField] private float delay = 3f; // Delay before instantiation

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Check if the player has entered the collider
        {
            Debug.LogWarning("Player entered MimicTrigger");
            StartCoroutine(InstantiateAfterDelay(other.transform.position));
        }
    }

    private IEnumerator InstantiateAfterDelay(Vector3 spawnPosition)
    {
        // Wait for the specified delay
        yield return new WaitForSeconds(delay);
        
        // Instantiate the prefab at the player's position
        Instantiate(prefabToInstantiate, spawnPosition, Quaternion.identity);

        // Destroy the specified target GameObject
        if (targetToDestroy != null)
        {
            Destroy(targetToDestroy);
            Debug.LogWarning("Destroyed target GameObject: " + targetToDestroy.name);
        }
        else
        {
            Debug.LogWarning("No target GameObject assigned to destroy.");
        }
    }
}
