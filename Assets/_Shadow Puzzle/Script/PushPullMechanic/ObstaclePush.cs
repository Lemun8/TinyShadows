using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstaclePush : MonoBehaviour
{
    public float distance = 1f;
    public LayerMask boxMask;

    private GameObject box;
    private PlayerController playerController;
    private bool isPushing = false;

    private void Start()
    {
        playerController = GetComponent<PlayerController>(); // Reference to PlayerController
    }

    private void Update()
    {
        RaycastHit hit;

        // Detect object and check if the player presses "Spacebar" to toggle pushing
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!isPushing) // Start pushing
            {
                if (Physics.Raycast(transform.position, transform.forward, out hit, distance, boxMask))
                {
                    if (hit.collider != null && hit.collider.gameObject.CompareTag("Pushable"))
                    {
                        box = hit.collider.gameObject;
                        isPushing = true;

                        // Start pushing
                        playerController.StartPushing(hit.transform.position - transform.position);

                        // Attach a FixedJoint to the box for pushing
                        FixedJoint joint = box.GetComponent<FixedJoint>();
                        if (joint == null)
                        {
                            joint = box.AddComponent<FixedJoint>();
                        }
                        joint.connectedBody = GetComponent<Rigidbody>();
                        box.GetComponent<BoxPull>().beingPushed = true;
                    }
                }
            }
            else // Stop pushing
            {
                StopPushing();
            }
        }
    }

    private void StopPushing()
    {
        if (isPushing && box != null)
        {
            isPushing = false;

            // Stop pushing
            playerController.StopPushing();

            // Remove the joint to stop pushing
            FixedJoint joint = box.GetComponent<FixedJoint>();
            if (joint != null)
            {
                Destroy(joint);
            }
            box.GetComponent<BoxPull>().beingPushed = false;
            box = null;
        }
    }
}
