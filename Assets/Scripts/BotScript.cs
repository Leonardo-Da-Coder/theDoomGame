using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BotScript : MonoBehaviour
{
    public NavMeshAgent botController;

    public Animator animator;

    public Transform player;

    public LayerMask groundMask, playerMask;

    [Header("Patroling")]
    public Vector3 walkPoint;
    bool setWalkPoint;
    public float walkPointRange;

    [Header("Attacking")]
    public float sightRange, AttackRange;
    public bool playerInSightRange, playerInAttackRange;

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        botController = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        Patroling();
    }

    private void Patroling()
    {
        if (!setWalkPoint) SearchWalkPoint();

        if (setWalkPoint) botController.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        if (distanceToWalkPoint.magnitude < 1f) setWalkPoint = false;
    }

    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up,2f, groundMask)) setWalkPoint = true;

    }
}
