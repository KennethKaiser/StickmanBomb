using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementPlayer : MonoBehaviour
{

    public bool Right;
    public bool Left;

    public Rigidbody2D rbRIGHT;
    public Rigidbody2D rbLEFT;
    public Rigidbody2D body;
    public Rigidbody2D rightArmRb;
    public Rigidbody2D rightArmRb2;

    public Vector2 WalkRightVector;
    public Vector2 WalkLeftVector;

    public bool grounded;
    public bool jumped;
    public bool inTheAir;
    public float positionRadius;
    public LayerMask ground;
    public Transform playerPos;
    [SerializeField]
    private float jumpForce = 60f;
    [SerializeField]
    private float airForce = 40f;
    public float stabilizationCooldown = 0f;
    private const float stabilizationCooldownDuration = 0.2f; // 0.2 seconds, adjust as necessary

    private bool step1RightReady = false;
    private bool step2RightReady = false;
    private bool step1LeftReady = false;
    private bool step2LeftReady = false;

    private bool cycleRight = true;
    private bool cycleLeft = true;

    //0.085f original
    [SerializeField]
    private float timeBetweenSteps = 0.3f;

    public bool _noMovement;
    public bool noMovement { get { return _noMovement; } }

    public BombBehavior bombBehavior;

    private float armAngle1;
    private float armAngle2;
    [SerializeField]
    private float armRotationSpeed = 5f;


    [System.Serializable]
    public class Limbs
    {
        public Rigidbody2D rigidbody2D;
        public float targetRotation;
    }

    // List of body parts
    public List<Limbs> limbs = new List<Limbs>();

    void Start()
    {
        
    }

    
    void Update()
    {
        CalculateArmMovement();
        InputChecker();
        StabCooldown();
        GroundChecker();

        if (!grounded)
        {
            if (!bombBehavior.playerInTheAir)
            {
                AirMovement();
            }
            
            return;
        } 

        

        if (!Left && !Right)
        {
            _noMovement = true;
        }
        else
        {
            _noMovement = false;
        }


        while (Right == true && Left == false && cycleRight)
        {
            Invoke("Step1RightRdy", 0f);
            Invoke("Step2RightRdy", timeBetweenSteps);
            Invoke("CycleRight", 2 * timeBetweenSteps);

            cycleRight = false;
        }

        while (Left == true && Right == false && cycleLeft)
        {
            Invoke("Step1LeftRdy", 0f);
            Invoke("Step2LeftRdy", timeBetweenSteps);
            Invoke("CycleLeft", 2 * timeBetweenSteps);

            cycleLeft = false;
        }

        JumpKeyPressed();

    }

    private void FixedUpdate()
    {
        MoveArmToMouse();

        if (step1RightReady)
        {
            Step1Right();
            step1RightReady = false;
        }

        if (step2RightReady)
        {
            Step2Right();
            step2RightReady = false;
        }

        if (step1LeftReady)
        {
            Step1Left();
            step1LeftReady = false;
        }

        if (step2LeftReady)
        {
            Step2Left();
            step2LeftReady = false;
        }

        Jump();

    }

    public void CalculateArmMovement()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 armPosition1 = rightArmRb.position;
        Vector2 direction1 = mousePosition - armPosition1;

        Vector2 armPosition2 = rightArmRb2.position;
        Vector2 direction2 = mousePosition - armPosition2;


        armAngle1 = Mathf.Atan2(direction1.y, direction1.x) * Mathf.Rad2Deg;
        armAngle2 = Mathf.Atan2(direction2.y, direction2.x) * Mathf.Rad2Deg;


        Debug.Log(armAngle1);
        Debug.Log(armAngle2);

    }

    public void MoveArmToMouse()
    {
        float newRotation = Mathf.LerpAngle(rightArmRb.rotation, armAngle1, 1);
        rightArmRb.MoveRotation(newRotation);

        float newRotation2 = Mathf.LerpAngle(rightArmRb.rotation, armAngle2, 1);
        rightArmRb2.MoveRotation(newRotation2);
    }


    public void AirMovement()
    {
        if (Right && !Left)
        {
            body.AddForce(Vector2.right * airForce, ForceMode2D.Force);
        }
        else if(Left && !Right)
        {
            body.AddForce(Vector2.left * airForce, ForceMode2D.Force);
        }
    }

    public void InputChecker()
    {
        if (Input.GetKey(KeyCode.D))
        {
            Right = true;
        }
        else
        {
            Right = false;
        }

        if (Input.GetKey(KeyCode.A))
        {
            Left = true;
        }
        else
        {
            Left = false;
        }
    }
    public void Jump()
    {
        if (grounded && jumped)
        {
            inTheAir = true;
            foreach (Limbs limb in limbs)
            {
                Vector2 newVelocity = limb.rigidbody2D.velocity;
                newVelocity.y = 0;
                limb.rigidbody2D.velocity = newVelocity;
            }
            stabilizationCooldown = stabilizationCooldownDuration;

            body.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            jumped = false;


        }
    }

    public void StabCooldown()
    {
        if (stabilizationCooldown > 0)
        {
            stabilizationCooldown -= Time.deltaTime;
            if (stabilizationCooldown < 0)
                stabilizationCooldown = 0;
        }
    }

    public void GroundChecker()
    {
        grounded = Physics2D.OverlapCircle(playerPos.position, positionRadius, ground);
    }

    public void JumpKeyPressed()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumped = true;
        }
    }


    private void CycleLeft()
    {
        cycleLeft = true;
    }

    private void CycleRight()
    {
        cycleRight = true;
    }

    public void Step1RightRdy()
    {
        step1RightReady = true;
    }

    public void Step2RightRdy()
    {
        step2RightReady = true;
    }

    public void Step1LeftRdy()
    {
        step1LeftReady = true;
    }

    public void Step2LeftRdy()
    {
        step2LeftReady = true;
    }


    public void Step1Right()
    {
        rbRIGHT.AddForce(WalkRightVector, ForceMode2D.Impulse);
        rbLEFT.AddForce(WalkRightVector * -0.5f, ForceMode2D.Impulse);
    }

    public void Step2Right()
    {
        rbLEFT.AddForce(WalkRightVector, ForceMode2D.Impulse);
        rbRIGHT.AddForce(WalkRightVector * -0.5f, ForceMode2D.Impulse);
    }

    public void Step1Left()
    {
        rbRIGHT.AddForce(WalkLeftVector, ForceMode2D.Impulse);
        rbLEFT.AddForce(WalkLeftVector * -0.5f, ForceMode2D.Impulse);
    }

    public void Step2Left()
    {
        rbLEFT.AddForce(WalkLeftVector, ForceMode2D.Impulse);
        rbRIGHT.AddForce(WalkLeftVector * -0.5f, ForceMode2D.Impulse);
    }



}
