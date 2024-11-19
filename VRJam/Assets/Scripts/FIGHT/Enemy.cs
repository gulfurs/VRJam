using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{

    [Header("References")]
    public NavMeshAgent agent;
    public Animator animator;
    public Transform player;
    public SentenceCreation sentenceCreation;

    [Header("COMBAT")]
    public float attackRange = 2f;
    public float chaseRange = 50f;
    public float attackCooldown = 2f;
    public float dmg = 10f;
    public int maxHP = 100;

    [Header("Animations")]
    private const string ENEMY_WALK = "Walk";
    private const string ENEMY_ATTACK = "Attack";
    private const string ENEMY_IDLE = "Idle";

    private enum EnemyState {Idle, Chasing, Attacking}
    private EnemyState currentState = EnemyState.Idle;

    private float lastAttTime;
    private int currentHP;

    void Start(){

        //Initialize
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        sentenceCreation = FindObjectOfType<SentenceCreation>();

        //Health
        currentHP = maxHP;

        gameObject.SetActive(false);

    }

    //Called from SentenceCreation
    public void ReleaseEnemy(){
        gameObject.SetActive(true);
        currentState = EnemyState.Chasing;
    }

    void Update(){

        if (currentState == EnemyState.Idle) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        switch (currentState){
            case EnemyState.Chasing:
                ChasePlayer(distanceToPlayer);
                break;
            case EnemyState.Attacking:
                AttPlayer(distanceToPlayer);
                break;
        }

    }

    void ChasePlayer(float distance){
        
        if (distance <= attackRange){
            currentState = EnemyState.Attacking;
            agent.isStopped = true;
            animator.SetTrigger(ENEMY_ATTACK);
            return;
        } 

        if (distance <= chaseRange){
            agent.SetDestination(player.position);
            agent.isStopped = false;
            animator.SetBool(ENEMY_WALK, true);
        } else {
            agent.isStopped = true;
            animator.SetBool(ENEMY_WALK, false);
        }
    }

    void AttPlayer(float distance){
        transform.LookAt(player); //If no work = Remove

        if (distance > attackRange){
            currentState = EnemyState.Chasing;
            return;
        }

        //ATTACKING
        if (Time.time >= lastAttTime + attackCooldown){
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth != null){
                playerHealth.TakeDmg(dmg);
                lastAttTime = Time.time;
                animator.SetTrigger(ENEMY_ATTACK);
            }
        }
    }

    public void TakeDmg(float dmgAmount){
        currentHP -= (int)dmgAmount;

        if (currentHP <= 0){
            Die();
        }
    }

    void Die(){
        animator.SetTrigger("Die");
        agent.isStopped = true;
        currentState = EnemyState.Idle;

        Invoke("Vanish", 2f);
    }

    void Vanish(){
        gameObject.SetActive(false);
    }

}
