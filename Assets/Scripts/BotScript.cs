using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BotScript : MonoBehaviour
{
    public NavMeshAgent botController;

    public Animator animator;

    public GameObject gunTip;
    public GameObject gun;

    public Transform camera;

    public LayerMask groundMask, playerMask;

    [Header("Patroling")]
    public Vector3 walkPoint;
    bool setWalkPoint;
    public float walkPointRange;

    [Header("Attacking")]
    public float timeBetweenAttacks;
    public GameObject projectile;
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange, alreadyAttacked;

    public bool isWalking, isHurt, isDead, isAttacking;

    private void Awake()
    {
        botController = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, playerMask);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, playerMask);

        if (!playerInSightRange && !playerInAttackRange && !isDead) {
            Patroling();
            isWalking = true;
            isAttacking = false;
            animator.SetBool("isAttacking", isAttacking);
            animator.SetBool("isWalking", isWalking);
        }

        if (playerInSightRange && !playerInAttackRange && !isDead && !isHurt) {
            isWalking = true;
            isAttacking = false;
            animator.SetBool("isAttacking", isAttacking);
            animator.SetBool("isWalking", isWalking);
            ChasePlayer();
        } 
        
        if (playerInSightRange && playerInAttackRange && !isDead && !isHurt ) {
            isWalking = false;
            animator.SetBool("isWalking", isWalking);
            isAttacking = true;
            animator.SetBool("isAttacking", isAttacking);

            AttackPlayer();
        }




    }

    private void Patroling()
    {
        if (isHurt)
        {
            botController.SetDestination(transform.position);
            return;
        }
        if (!setWalkPoint) SearchWalkPoint();

        if (setWalkPoint)
        {
            
            botController.SetDestination(walkPoint);
        } 

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        if (distanceToWalkPoint.magnitude < 1f) setWalkPoint = false;
    }

    private void ChasePlayer()
    {
        if (isHurt) {
            botController.SetDestination(transform.position);
            return;
        } 
        botController.SetDestination(camera.position);
    }

    private void AttackPlayer()
    {
        if (isHurt) return;
        botController.SetDestination(transform.position);

        transform.LookAt(camera);
        gunTip.transform.LookAt(camera);
        gun.transform.LookAt(camera);

        if (!alreadyAttacked)
        {
            Vector3 shootDirection = camera.transform.position - gunTip.transform.position;

            Rigidbody rb = Instantiate(projectile, gunTip.transform.position, Quaternion.identity).GetComponent<Rigidbody>();

            rb.AddForce(shootDirection.normalized * 5f, ForceMode.Impulse);
            



            alreadyAttacked = true;
            Invoke("ResetAttack", timeBetweenAttacks);
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up,2f, groundMask)) setWalkPoint = true;

    }

    public int health;

    public void TakeDamage(int damage)
    {
        health -= damage;
        
        if (health <= 0)
        {
            Dead();
        }
        else
        {
            isHurt = true;
            isAttacking = false;
            animator.SetBool("isHurt", isHurt);
            botController.SetDestination(transform.position);

            Invoke("ResetHurt", 1.767f);
        }
    }

    private void Dead()
    {
        botController.SetDestination(transform.position);

        isWalking = false;
        isAttacking = false;
        isHurt = false;
        isDead = true;

        animator.SetBool("isWalking", isWalking);
        animator.SetBool("isAttacking", isAttacking);
        animator.SetBool("isHurt", isHurt);
        animator.SetBool("isDead", isDead);

    }

    private void ResetHurt()
    {
        isHurt = false;
        animator.SetBool("isHurt", isHurt);

    }


}
