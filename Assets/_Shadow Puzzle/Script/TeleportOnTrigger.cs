using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TeleportOnTrigger : MonoBehaviour
{
    [SerializeField] private GameObject goTo;
    [SerializeField] private GameObject target;

    public bool isTriggered;

    private void Awake()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isTriggered)
        {
            OnTeleport();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && isTriggered)
        {
            //target.GetComponent<CharacterController>().enabled = true;
            //target.GetComponent<PlayerInput>().enabled = true;
            isTriggered = false;
        }
    }

    public virtual void OnTeleport()
    {
        target.GetComponent<CharacterController>().enabled = false;
        target.GetComponent<PlayerInput>().enabled = false;
        Debug.Log("TP");
        isTriggered = true;
        target.transform.position = goTo.transform.position;

        target.GetComponent<CharacterController>().enabled = true;
        target.GetComponent<PlayerInput>().enabled = true;
        isTriggered = false;
    }
}
