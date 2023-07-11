using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public NavMeshAgent agent;

    public Transform player;

    public LayerMask whatIsGround, whatIsPlayer;

    //Patroling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    public GameObject projectile;

    //States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    public Transform RightHand;
    Animator AnimatorAI;
    float AnimationAcceleration = 1f, velocityX = 0f;

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        AnimatorAI = GetComponent<Animator>();
    }

    private void Update()
    {
        //Check for sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInSightRange && !playerInAttackRange) Patroling();
        if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        if (playerInAttackRange && playerInSightRange) AttackPlayer();

        if(walkPointSet)
        {
            Vector3 Vector = (walkPoint - transform.position);
            Vector.y = 0;

            if(velocityX < 1)
                velocityX += (AnimationAcceleration * Time.deltaTime);
            else
                velocityX = 1;
        }
        else
        {
            if(velocityX > 0)
                velocityX -= (AnimationAcceleration * Time.deltaTime);
            else
                velocityX = 0;
        }

        AnimatorAI.SetFloat("Velocity X", velocityX);
    }

    private void Patroling()
    {
        
        if (!walkPointSet)
            SearchWalkPoint();

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //Walkpoint reached
        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;

        if (walkPointSet)
            agent.SetDestination(walkPoint);
    }
    private void SearchWalkPoint()
    {
        //Calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
        {
            walkPointSet = true;
        }
        else
        {
            transform.Translate(Vector3.up * -10 * Time.deltaTime);
        }

    }

    private void ChasePlayer()
    {
        walkPoint = player.position;
        walkPointSet = true;
        agent.SetDestination(walkPoint);
    }

    private void AttackPlayer()
    {
        //Make sure enemy doesn't move
        agent.SetDestination(transform.position);
        walkPointSet = false;

        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            ///Attack code here
            Rigidbody rb = Instantiate(projectile, RightHand.position, Quaternion.identity).GetComponent<Rigidbody>();

            Vector3 AttackPosition = (player.position - RightHand.position);

            AttackPosition.y += 2f;

            rb.AddForce(AttackPosition * 2f, ForceMode.VelocityChange);

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }
    private void ResetAttack()
    {
        alreadyAttacked = false;
    }
}
