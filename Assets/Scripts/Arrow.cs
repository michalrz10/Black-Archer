using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    private Rigidbody rb;
    public bool moving;
    private AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        moving = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (moving)
        {
            if(rb==null) rb = gameObject.AddComponent<Rigidbody>();
            transform.rotation = Quaternion.LookRotation(rb.velocity);// * Quaternion.Euler(0,-90,0);
        }
    }

    public void addRigidbody()
    {
        moving = true;
        gameObject.transform.parent = null;
        rb = gameObject.AddComponent<Rigidbody>();
        rb.velocity = -gameObject.transform.up;
        GetComponent<CapsuleCollider>().enabled = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        moving = false;
        transform.rotation = Quaternion.LookRotation(rb.velocity);
        Destroy(rb);
        GetComponent<CapsuleCollider>().enabled = false;
        transform.parent = collision.transform;
        audioSource.Play();
    }
}
