using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public GameObject leftLeg;
    public GameObject rightLeg;
    Rigidbody2D leftLegRB;
    Rigidbody2D rightLegRB;
    public Rigidbody2D rb;


    public Animator anim;

    [SerializeField]
    private float speed;
    [SerializeField]
    private float jumpForce;

    public bool grounded;
    public float positionRadius;
    public LayerMask ground;
    public Transform playerPos;

    private Rigidbody2D[] limbs;

    [SerializeField]
    private float stepWait;

    private bool moving;

    public bool IsMoving { get { return moving; } }


    // Start is called before the first frame update
    void Start()
    {
        moving = false;
        leftLegRB = leftLeg.GetComponent<Rigidbody2D>();
        rightLegRB = rightLeg.GetComponent<Rigidbody2D>();


        limbs = GetComponentsInChildren<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetAxisRaw("Horizontal") != 0)
        {
            moving = true;
            if(Input.GetAxisRaw("Horizontal") > 0)
            {
                anim.Play("WalkRight");
                StartCoroutine(MoveRight(stepWait));
            }
            else
            {
                anim.Play("WalkLeft");
                StartCoroutine(MoveLeft(stepWait));
            }
        }
        else
        {
            moving = false;
            anim.Play("Idle");
        }

        grounded = Physics2D.OverlapCircle(playerPos.position, positionRadius, ground);
        if(grounded && Input.GetKeyDown(KeyCode.Space))
        {
            foreach (Rigidbody2D rigidbody2D in limbs)
            {
                Vector2 newVelocity = rigidbody2D.velocity;
                newVelocity.y = 0;
                rigidbody2D.velocity = newVelocity;
            }

            rb.AddForce(Vector2.up * jumpForce);
        }

    }

    IEnumerator MoveRight(float seconds)
    {
        leftLegRB.AddForce(Vector2.right * (speed), ForceMode2D.Force);
        yield return new WaitForSeconds(seconds);
        rightLegRB.AddForce(Vector2.right * (speed), ForceMode2D.Force);
    }

    IEnumerator MoveLeft(float seconds)
    {
        rightLegRB.AddForce(Vector2.left * (speed), ForceMode2D.Force);
        yield return new WaitForSeconds(seconds);
        leftLegRB.AddForce(Vector2.left * (speed), ForceMode2D.Force);
    }

}
