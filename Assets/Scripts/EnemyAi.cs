using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAi : MonoBehaviour
{
    public Animator animator;
    private NavMeshAgent agent;
    private Transform player;
    private int health;
    private int stage;
    public float sightRange=10f, runRange=6.5f, attackRange=1.6f;

    // Start is called before the first frame update
    void Start()
    {
        health = 2;
        stage = 0;
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(player.position, transform.position);
        if(stage == 0)
        {
            if (distance <= sightRange)
            {
                stage = 1;
                animator.SetBool("Walk", true);
            }
        }
        if(stage == 1)
        {
            Move();
            if (distance <= runRange)
            {
                animator.SetBool("Run", true);
                agent.SetDestination(gameObject.transform.position);
            }
        }
        if(stage == 3)
        {
            Run();
            if (distance <= attackRange) animator.SetBool("Attack", true);
        }

    }

    private void Move()
    {
        agent.SetDestination(player.position);
        agent.speed = 1.5f;
    }

    private void Run()
    {
        agent.SetDestination(player.position);
        agent.speed = 3f;
    }

    public void OnScreamEnd()
    {
        stage = 3;
    }

    public void OnAttack()
    {
        if(Vector3.Distance(player.position, transform.position) <= 2f)
        {
            GameObject.Find("Player").GetComponent<PlayerMovement>().Die();
        }
        animator.SetBool("Attack", false);
    }

    public void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Arrow"))
        {
            health -= 1;
            stage = 4;
            animator.SetBool("Walk", true);
            animator.SetBool("Run", true);
            if (health == 0)
            {
                Destroy(agent);
                animator.SetBool("Death", true);
                animator.SetBool("Death anim", Random.value>0.5f);
                for (int i = 0; i < transform.childCount; i++)
                {
                    if (transform.GetChild(i).CompareTag("Arrow"))
                    {
                        transform.GetChild(i).GetComponent<Arrow>().addRigidbody();
                    }
                }
            }
        }
    }

    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, sightRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, runRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
