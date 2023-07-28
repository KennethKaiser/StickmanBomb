using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombBehavior : MonoBehaviour
{
    private Rigidbody2D rb;

    public Transform armTransform;
    public Rigidbody2D armRigidbody;

    private bool isFollowing = true;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = true;
    }

    void Update()
    {
        if (isFollowing)
        {
            transform.position = armTransform.position;
            transform.rotation = armTransform.rotation;
        }

        // Checking if the left mouse button was pressed
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            // If the Rigidbody is kinematic (not falling), start falling
            if (rb.isKinematic)
            {
                // Assign the velocity of the arm to the bomb
                rb.velocity = armRigidbody.velocity;
                rb.angularVelocity = armRigidbody.angularVelocity;

                rb.isKinematic = false;
                isFollowing = false;
            }
            else // If the Rigidbody is not kinematic (falling), stop falling
            {
                rb.velocity = Vector2.zero; // Reset velocity
                rb.angularVelocity = 0f; // Reset angular velocity

                rb.isKinematic = true;
                isFollowing = true;
            }
        }
    }

    IEnumerator StopFollowing()
    {
        isFollowing = false;
        yield return new WaitForSeconds(5f);
        rb.velocity = Vector2.zero; // Reset velocity
        rb.angularVelocity = 0f; // Reset angular velocity
        isFollowing = true;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the bomb has collided with the ground
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            StartCoroutine(SlowDown());
        }
    }

    IEnumerator SlowDown()
    {
        float timer = 0f;
        Vector2 initialVelocity = rb.velocity;

        while (timer < 2f)
        {
            // Gradually decrease the velocity over 2 seconds
            rb.velocity = Vector2.Lerp(initialVelocity, Vector2.zero, timer / 2f);
            timer += Time.deltaTime;
            yield return null;
        }

        // Make sure velocity is zero after 2 seconds
        rb.velocity = Vector2.zero;
    }
}
