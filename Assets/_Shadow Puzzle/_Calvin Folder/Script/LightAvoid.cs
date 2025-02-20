using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LightAvoid : MonoBehaviour
{
    [Header("AI Setting")]
    [SerializeField]
    private NavMeshAgent agent;
    [SerializeField]
    private Transform chaser;
    [SerializeField]
    private Transform player;
    [SerializeField]
    private float displacementDist = 5f;

    public GameObject[] allShadow;
    [SerializeField]
    private GameObject nearestObject;
    private float distance;
    public float nearestDistance;
    private GameObject prevObject;

    [Header("Detection Settings")]
    [SerializeField]
    private float detectionRange = 10f;

    public GameObject deathScreen;
    private bool hasTrigger;

    public AudioSource onDeath;

    void Start()
    {
        allShadow = GameObject.FindGameObjectsWithTag("Shadow");
        if (agent == null)
            if (!TryGetComponent(out agent))
                Debug.LogWarning(name + " needs a NavMeshAgent.");
    }

    private void Update()
    {
        if (chaser != null)
            NearestShadow();
        else
            MoveToTarget();

        if(agent.remainingDistance <= agent.stoppingDistance)
        {
            detectionRange = 5;
        }
    }

    private void MoveToTarget()
    {
        agent.speed = 8f;
        if (Vector3.Distance(transform.position, player.position) <= detectionRange)
        {
            agent.SetDestination(player.position);
            agent.isStopped = false;
        }
    }

    private void NearestShadow()
    {
        agent.speed = 9;
        detectionRange = 0;
        GameObject closest = null;
        float closestDistance = float.MaxValue;

        foreach (GameObject shadow in allShadow)
        {
            if (shadow == prevObject) continue;

            distance = Vector3.Distance(transform.position, shadow.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closest = shadow;
            }
        }

        if (closest != null && closest != nearestObject)
        {
            prevObject = nearestObject;
            nearestObject = closest;
            agent.SetDestination(nearestObject.transform.position);
            //agent.isStopped = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Light") && hasTrigger == false)
        {
            prevObject = nearestObject;
            hasTrigger = true;
            chaser = other.transform;
        }
        if (other.CompareTag("Player"))
        {
            OnPlayerDeath();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Light") && hasTrigger == true)
        {
            hasTrigger = false;
            chaser = null;
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
        Gizmos.DrawWireSphere(transform.position, detectionRange);

    }
}
