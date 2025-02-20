using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactivateColliderOnTrigger : MonoBehaviour
{
    private bool isTriggered;
    private bool wait;
    [SerializeField] private Collider targetCollider;
    public BoxPull boxPull;
    float time;


    private void Start()
    {
        var parent = transform.parent;
        boxPull= parent.GetChild(0).GetComponent<BoxPull>();
    }
    private void LateUpdate()
    {
        if (wait)
        {
            time += Time.deltaTime;
            if(time >3f)
            {
                Debug.LogWarning("Failsafe");
                targetCollider.enabled = true;
                isTriggered = false;
                wait = false;
            }
        }
        else
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
                if (!boxPull.beingPushed)
                {
                    Debug.LogWarning("Enter with box pull");
                    targetCollider.enabled = false;
                    isTriggered = true;
                }
            }
            else if(boxPull == null)
            {
                Debug.LogWarning("Enter");
                targetCollider.enabled = false;
                isTriggered = true;
            }


        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.LogWarning("Exit");
        //targetCollider.enabled = true;
        //isTriggered = false;
        wait= true;
    }
}
