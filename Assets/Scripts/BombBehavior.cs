using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombBehavior : MonoBehaviour
{
    public GameObject stickman;
    public GameObject body;
    private Rigidbody2D[] limbs;

    private Rigidbody2D rb;

    public Transform armTransform;
    public Rigidbody2D armRigidbody;

    public Rigidbody2D bodyRB;

    private bool isFollowing = true;

    //Explosion
    [SerializeField]
    private float explosionForce;

    private bool hitGround = false;

    private SpriteRenderer[] boxes;
    private SpriteRenderer[] allRenders;
    private SpriteRenderer parentRenderer;

    public CircleTriggerHandler innerCircles;
    public CircleTriggerHandler outerCircles;
    



    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = true;
        parentRenderer = GetComponent<SpriteRenderer>();
        allRenders = GetComponentsInChildren<SpriteRenderer>();
        boxes = new SpriteRenderer[allRenders.Length - 1];

        int i = 0;
        for (int j = 0; j < allRenders.Length; j++)
        {
            if(allRenders[j] != parentRenderer)
            {
                boxes[i] = allRenders[j];
                boxes[i].enabled = false;
                i++;
            }
            
        }
        limbs = stickman.GetComponentsInChildren<Rigidbody2D>();
        

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

                StartCoroutine(ExplosionHandler());
            }
        }
    }


    IEnumerator ExplosionHandler()
    {
        StartCoroutine(Explosion());
        yield return new WaitForSeconds(1.0f); // Wait for 1 second
        parentRenderer.enabled = true;
        rb.isKinematic = true;
        isFollowing = true;
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
            foreach(SpriteRenderer spriteRenderer in boxes)
            {
                spriteRenderer.enabled = true;
                Debug.Log(spriteRenderer.enabled);
            }
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

    IEnumerator WaitForLanding(OnTheGround onTheGround, Balance[] balanceScripts)
    {
        yield return new WaitUntil(() => onTheGround.IsGrounded); // Wait until the character is on the ground
        
        foreach (Balance balanceScript in balanceScripts)
        {
            balanceScript.enabled = true;
        }

    }

    IEnumerator Explosion()
    {
        //Turn off bomb
        foreach (SpriteRenderer sprite in allRenders)
        {
            sprite.enabled = false;
        }



        float totalForceMultiplier = 0f;

        if (innerCircles.IsLimbInside)
        {
            Debug.Log("Inner circle hit");
            totalForceMultiplier = innerCircles.forceMultiplier;
        }
        else if (outerCircles.IsLimbInside)
        {
            Debug.Log("Outer circle hit");
            totalForceMultiplier = outerCircles.forceMultiplier;
        }
        else
        {
            Debug.Log("We are not hit by the bomb");
            yield break;
        }


        var dir = (bodyRB.transform.position - transform.position);

        bodyRB.AddForce(dir.normalized * totalForceMultiplier, ForceMode2D.Impulse);

        // Disable all Balance scripts in the children of the stickman
        Balance[] balanceScripts = stickman.GetComponentsInChildren<Balance>();
        foreach (Balance balanceScript in balanceScripts)
        {
            balanceScript.enabled = false;
        }

        

        yield return new WaitForSeconds(0.4f);

        

        //Hit the ground again
        OnTheGround onTheGround = body.GetComponent<OnTheGround>();
        StartCoroutine(WaitForLanding(onTheGround, balanceScripts));

    }

}
