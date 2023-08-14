using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalancePlay : MonoBehaviour
{
    private Rigidbody2D rb;
    public Rigidbody2D rb2;
    [SerializeField]
    private float targetRotation = 0f;

    [SerializeField]
    private float changeRate = 20.0f;
    [SerializeField]
    private float maxRotation = 10.0f;
    [SerializeField]
    private float minRotation = -10.0f;

    public bool tester = false;

    public float rotationSpeed = 1f;

    public float forceMagnitude = 5.0f;


    private float armAngle1;
    private float armAngle2;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleRotationInput();
        CalculateArmMovement();
    }

    private void FixedUpdate()
    {
        rb.MoveRotation(targetRotation);
        rb2.MoveRotation(targetRotation);
        MoveArmToMouse();

    }

    void HandleRotationInput()
    {
        if (Input.GetKey(KeyCode.E))
        {
            targetRotation += changeRate * Time.deltaTime;
            targetRotation = Mathf.Clamp(targetRotation, minRotation, maxRotation);
        }
        else if (Input.GetKey(KeyCode.Q))
        {
            targetRotation -= changeRate * Time.deltaTime;
            targetRotation = Mathf.Clamp(targetRotation, minRotation, maxRotation);
        }
        else
        {
            if (tester)
            {
                ApplyStabilizingForce();
            }
            
        }

    }

    public void CalculateArmMovement()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 armPosition1 = rb.position;
        Vector2 direction1 = mousePosition - armPosition1;

        Vector2 armPosition2 = rb2.position;
        Vector2 direction2 = mousePosition - armPosition2;


        armAngle1 = Mathf.Atan2(direction1.y, direction1.x) * Mathf.Rad2Deg;
        armAngle2 = Mathf.Atan2(direction2.y, direction2.x) * Mathf.Rad2Deg;


    }

    public void MoveArmToMouse()
    {
        float newRotation = Mathf.LerpAngle(rb.rotation, armAngle1, 1);
        rb.MoveRotation(newRotation);

        float newRotation2 = Mathf.LerpAngle(rb2.rotation, armAngle2, 1);
        rb2.MoveRotation(newRotation2);
    }

    void ApplyStabilizingForce()
    {
        // Find the current velocity
        Vector2 currentVelocity = rb.velocity;
        Vector2 currentVelocity2 = rb2.velocity;

        // Define a stabilizing force magnitude (you'll need to tweak this value).


        // Apply the opposite force to counteract the movement.
        if (currentVelocity.magnitude > 0.05f)  // Only apply force if there's significant movement
        {
            Debug.Log("WE ARE ADDING FORCE");
            rb.AddForce(-currentVelocity * forceMagnitude);

        }

        if (currentVelocity2.magnitude > 0.05f)  // Only apply force if there's significant movement
        {
            Debug.Log("WE ARE ADDING FORCE 2");
            rb2.AddForce(-currentVelocity2 * forceMagnitude);

        }
    }


    private void OnGUI()
    {
        string displayText = "current AngularVelocity: " + rb.velocity;

        GUI.Label(new Rect(10, 10, 500, 200), displayText);
    }


}
