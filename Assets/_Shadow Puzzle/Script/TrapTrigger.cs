using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapTrigger : MonoBehaviour
{
    [SerializeField] private Transform respawnPoint;
    [SerializeField] private BoxPull boxPull;
    public bool isTriggered;
    private float time;

    private void Start()
    {
        var parent = transform.parent;
        boxPull = parent.GetChild(0).GetComponent<BoxPull>();
    }

    private void LateUpdate()
    {
        // Reset timer if player has exited the trigger zone
        if (!isTriggered)
        {
            time = 0;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isTriggered)
        {

            if(boxPull != null)
            {
                if(!boxPull.beingPushed)
                {

                    // Call the method to handle player death and respawn
                    Debug.LogWarning("Player Entered Trap wiwth box pull");
                    isTriggered = true;
                    RespawnPlayer(other.gameObject);
                }
            }else if(boxPull == null)
            {
                Debug.LogWarning("Player Entered Trap");
                isTriggered = true;
                RespawnPlayer(other.gameObject);
            }

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isTriggered = false;
        }
    }

    private void RespawnPlayer(GameObject player)
    {
        // Example code to reset the player position to respawn point
        CharacterController controller = player.GetComponent<CharacterController>();
        if (controller != null)
        {
            controller.enabled = false; // Temporarily disable to prevent issues with CharacterController
            player.transform.position = respawnPoint.position;
            controller.enabled = true; // Re-enable after repositioning
        }
        else
        {
            player.transform.position = respawnPoint.position; // For non-CharacterController components
        }

        isTriggered = false;
        // Optionally reset any player state here (e.g., health)
        Debug.Log("Player died and respawned back at respawn point.");
    }
}
