using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxPull : MonoBehaviour
{
    public float defaultMass;
    public float immovableMass;
    public bool beingPushed;
    private float xPos;
    private Vector3 lastPos;

    public int mode;
    public int colliding;

    void Start()
    {
        xPos = transform.position.x;
        lastPos = transform.position;
    }

    void FixedUpdate()
    {
        if (mode == 0)
        {
            if (!beingPushed)
            {
                // Keep the box in the same x position if not being pushed
                transform.position = new Vector3(xPos, transform.position.y, transform.position.z);
            }
            else
            {
                xPos = transform.position.x;
            }
        }
        else if (mode == 1)
        {
            Rigidbody rb = GetComponent<Rigidbody>();

            if (!beingPushed)
            {
                // Set mass to immovable when not being pushed
                rb.mass = immovableMass;
            }
            else
            {
                // Restore default mass when being pushed
                rb.mass = defaultMass;
            }
        }
    }
}
