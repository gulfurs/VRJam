using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [Header("References")]
    public Animator animator;           
    public Transform player;             
    public SentenceCreation sentenceCreation;  

    [Header("Combat Settings")]
    public float attackRange = 2f;         
    public float attackCooldown = 2f;      
    public float damage = 10f;             
    public int maxHP = 100;                

    private enum EnemyState { Idle, Chasing, Attacking }
    private EnemyState currentState = EnemyState.Idle;
    private static readonly int StateParam = Animator.StringToHash("State");

    private float lastAttackTime;                
    private int currentHP;                       
    private Vector3 startPosition;  
    public float distanceToPlayer;   

    public bool isDead = false;

    EnemyManager enemyMan;

    void Start()
    {
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        sentenceCreation = FindObjectOfType<SentenceCreation>();
        enemyMan = FindFirstObjectByType<EnemyManager>();
        currentHP = maxHP;
        startPosition = transform.position;   // Store starting position
        //gameObject.SetActive(false);
    }

    // Called externally to release a new enemy
    public void ReleaseEnemy()
    {
        transform.position = startPosition;  // Reset position to the starting point
        currentHP = maxHP;                   // Reset health
        animator.SetFloat(StateParam, 0);
        //animator.SetBool(ENEMY_WALK, false); 
        gameObject.SetActive(true);          
        currentState = EnemyState.Chasing;   // Start chasing immediately
    }

    void Update()
    {
        distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (!isDead) {
        ChasePlayer(distanceToPlayer);
        AttackPlayer(distanceToPlayer);
        }
        //animator.SetFloat(StateParam, 0);

    }

    void AttackPlayer(float distance)
    {
        if (distance <= attackRange)
        {
            currentState = EnemyState.Attacking;
            //animator.SetTrigger(ENEMY_ATTACK);
            animator.SetFloat(StateParam, 2);
             Debug.Log("ATTACK ON RIVERDALE");
             if (Time.time >= lastAttackTime + attackCooldown)
        {
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDmg(damage);
                lastAttackTime = Time.time;  // Reset cooldown timer
                //animator.SetTrigger(ENEMY_ATTACK);
                animator.SetFloat(StateParam, 2);
            }
        }
            return;
        }
        //animator.SetBool(ENEMY_WALK, true);
        animator.SetFloat(StateParam, 1);
    }

    void ChasePlayer(float distance)
    {   
        if (distance > attackRange)
            {
            currentState = EnemyState.Chasing;  // Return to chasing if the player moves away
            animator.SetFloat(StateParam, 1);
            Debug.Log("CHASE YOU!!");
            return;
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
        isDead = true;
        animator.SetFloat(StateParam, -1);
        //currentState = EnemyState.Idle;

        //Invoke("ResetEnemy", 2f);  // Wait for death animation before resetting
        //sentenceCreation.Sword.SetActive(false);
    }

    public void DestroyEnemy()
    {
        enemyMan.HandleEnemyDestroyed(gameObject);
        Destroy(gameObject);  // Hide enemy until respawned externally
    }

    public bool IsDead()
    {
        return currentHP <= 0;
    }
}
