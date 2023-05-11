using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trampoline : MonoBehaviour
{
    public float jumpForce;
    public bool hasJumped;

    private void Start()
    {
        hasJumped = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasJumped)
        {
            hasJumped = true;
            other.GetComponent<PlayerMovement>().doubleJump = true;
            Rigidbody rb = other.GetComponent<Rigidbody>();

            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);

            Invoke("resetJumped", 2f);
        }
    }

    private void resetJumped()
    {
        hasJumped = false;
    }
}
