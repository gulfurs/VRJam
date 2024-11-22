using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [Header("References")]
    public NavMeshAgent agent;           
    public Animator animator;           
    public Transform player;             
    public SentenceCreation sentenceCreation;  

    [Header("Combat Settings")]
    public float attackRange = 2f;         
    public float attackCooldown = 2f;      
    public float damage = 10f;             
    public int maxHP = 100;                

    [Header("Animations")]
    private const string ENEMY_WALK = "Walk";
    private const string ENEMY_ATTACK = "Attack";
    private const string ENEMY_IDLE = "Idle";

    private enum EnemyState { Idle, Chasing, Attacking }
    private EnemyState currentState = EnemyState.Idle;

    private float lastAttackTime;                
    private int currentHP;                       
    private Vector3 startPosition;               // Store start position for respawning

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        sentenceCreation = FindObjectOfType<SentenceCreation>();

        currentHP = maxHP;
        startPosition = transform.position;   // Store starting position
        gameObject.SetActive(false);
    }

    // Called externally to release a new enemy
    public void ReleaseEnemy()
    {
        transform.position = startPosition;  // Reset position to the starting point
        currentHP = maxHP;                   // Reset health
        agent.isStopped = false;             // Ensure NavMeshAgent is active
        animator.SetBool(ENEMY_WALK, false); // Reset animations
        gameObject.SetActive(true);          
        currentState = EnemyState.Chasing;   // Start chasing immediately
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        switch (currentState)
        {
            case EnemyState.Chasing:
                ChasePlayer(distanceToPlayer);
                break;
            case EnemyState.Attacking:
                AttackPlayer(distanceToPlayer);
                break;
        }
    }

    void ChasePlayer(float distance)
    {
        if (distance <= attackRange)
        {
            currentState = EnemyState.Attacking;
            agent.isStopped = true;
            animator.SetTrigger(ENEMY_ATTACK);
            return;
        }

        agent.SetDestination(player.position);
        animator.SetBool(ENEMY_WALK, true);
    }

    void AttackPlayer(float distance)
    {
        if (distance > attackRange)
        {
            currentState = EnemyState.Chasing;  // Return to chasing if the player moves away
            agent.isStopped = false;
            return;
        }

        if (Time.time >= lastAttackTime + attackCooldown)
        {
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDmg(damage);
                lastAttackTime = Time.time;  // Reset cooldown timer
                animator.SetTrigger(ENEMY_ATTACK);
            }
        }
    }

    public void TakeDamage(float damageAmount)
    {
        currentHP -= (int)damageAmount;
        if (currentHP <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        animator.SetTrigger("Die");
        agent.isStopped = true;
        currentState = EnemyState.Idle;

        Invoke("ResetEnemy", 2f);  // Wait for death animation before resetting
    }

    void ResetEnemy()
    {
        gameObject.SetActive(false);  // Hide enemy until respawned externally
    }

    public bool IsDead()
    {
        return currentHP <= 0;
    }
}
