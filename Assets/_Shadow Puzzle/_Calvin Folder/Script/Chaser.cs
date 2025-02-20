using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Chaser : MonoBehaviour
{
    [SerializeField]
    private NavMeshAgent agent;

    [Header("Target Settings")]
    [SerializeField]
    private Transform primaryTarget;
    [SerializeField]
    private Transform secondaryTarget;

    [Header("Detection Settings")]
    [SerializeField]
    private float detectionRadius = 5f;
    [SerializeField]
    private float favorableDistanceThreshold = 10f;

    public GameObject deathScreen;
    private Transform currentTarget;
    public GameObject[] allLight;
    private GameObject nearestObject;
    private float distance;
    //private GameObject prevObject;

    public AudioSource onDeath;

    void Start()
    {
        if (agent == null && !TryGetComponent(out agent))
            Debug.LogWarning(name + " needs a NavMeshAgent.");

        allLight = GameObject.FindGameObjectsWithTag("Light");
    }

    void Update()
    {
        ChooseTarget();
        if (currentTarget != null)
        {
            MoveToTarget();
        }
        else
        {
            //agent.isStopped = true;
        }
    }

    private void ChooseTarget()
    {
        NearestLight();

        bool primaryInRange = primaryTarget != null && Vector3.Distance(transform.position, primaryTarget.position) <= detectionRadius;
        bool secondaryInRange = secondaryTarget != null && Vector3.Distance(transform.position, secondaryTarget.position) <= detectionRadius;

        if (nearestObject != null && Vector3.Distance(transform.position, nearestObject.transform.position) <= detectionRadius)
        {
            currentTarget = nearestObject.transform;
        }
        else if (secondaryInRange && Vector3.Distance(transform.position, secondaryTarget.position) <= favorableDistanceThreshold)
        {
            currentTarget = secondaryTarget;
        }
        else if (primaryInRange)
        {
            currentTarget = primaryTarget;
        }
        else
        {
            currentTarget = null;
        }
    }

    private void MoveToTarget()
    {
        agent.SetDestination(currentTarget.position);
        agent.isStopped = false;
    }

    private void NearestLight()
    {
        GameObject closest = null;
        float closestDistance = float.MaxValue;

        foreach (GameObject light in allLight)
        {

            distance = Vector3.Distance(transform.position, light.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closest = light;
            }
        }

        if (closest != null && closest != nearestObject)
        {
            nearestObject = closest;
            secondaryTarget = nearestObject.transform;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.LogWarning("Death");
            OnPlayerDeath();
        }
    }

    private void OnPlayerDeath()
    {
        onDeath.Play();
        Time.timeScale = 0;
        deathScreen.SetActive(true);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, favorableDistanceThreshold);
    }
}
