using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationSounds : MonoBehaviour
{
    public AudioClip step;
    public AudioClip jump;
    public AudioClip grabString;
    public AudioClip bowRelease;
    public AudioClip die;
    private AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Step()
    {
        audioSource.PlayOneShot(step);
    }

    private void GrabString()
    {
        audioSource.PlayOneShot(grabString);
    }

    private void Jump()
    {
        audioSource.PlayOneShot(jump);
    }

    private void BowRelease()
    {
        audioSource.PlayOneShot(bowRelease);
    }

    private void Die()
    {
        audioSource.PlayOneShot(die);
        GetComponent<Animator>().SetBool("Die", false);
    }

    private void Restart()
    {
        GetComponentInParent<PlayerMovement>().Restart();
    }
}
