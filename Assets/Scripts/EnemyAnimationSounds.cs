using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationSounds : MonoBehaviour
{
    public AudioClip attack;
    public AudioClip step;
    public AudioClip scream;
    public AudioClip die;
    private AudioSource audioSource;
    private EnemyAi ea;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        ea = GetComponentInParent<EnemyAi>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Step()
    {
        audioSource.PlayOneShot(step);
    }

    private void Attack()
    {
        audioSource.PlayOneShot(attack);
    }

    private void OnAttack()
    {
        ea.OnAttack();
    }

    private void OnScream()
    {
        ea.OnScreamEnd();
    }

    private void Scream()
    {
        audioSource.PlayOneShot(scream);
    }

    private void Die()
    {
        GetComponent<Animator>().SetBool("Death", false);
        GetComponent<Animator>().applyRootMotion = true;
        Destroy(GetComponentInParent<CapsuleCollider>());
        audioSource.PlayOneShot(die);
    }
}
