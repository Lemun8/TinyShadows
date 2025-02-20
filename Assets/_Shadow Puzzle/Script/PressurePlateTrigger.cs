using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlateTrigger : MonoBehaviour
{
    private bool isTriggered;
    [SerializeField] private GameObject targetGameObject;
    [SerializeField] private AudioSource pressurePlaterSFX;


    private void Start()
    {
    }
    private void LateUpdate()
    {
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.LogWarning("Door Open");
            pressurePlaterSFX.Play();
            targetGameObject.SetActive(false);
            
        }
        else
        {
            Debug.Log("Nigger");
        }
    }

    private void OnTriggerExit(Collider other)
    {
       GetComponent<BoxCollider>().enabled = false;
    }
}
