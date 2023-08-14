using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balance : MonoBehaviour
{
    

    

    [SerializeField]
    private float upwardForce;

    // Testing for enabling speed
    private float currentUpwardForce;
    private Coroutine increaseForceCoroutine;
    [SerializeField]
    private float duration = 1f;

    [SerializeField]
    private float durationTimer = 1f;

    float timer = 0;
    public float dragAmount = 5000f;
    public bool freeze = false;

    // Rotation test
    private float changeRate = 20.0f; // Rate of change per second
    private float maxRotation = 10.0f;
    private float minRotation = -10.0f;
    private float transitionDuration = 1.0f;
    public float rotationSpeed = 1f;

    public float forceMagnitude = 5.0f;

    private MovementPlayer movement;

    


    [System.Serializable]
    public class BodyPart
    {
        public Rigidbody2D rigidbody2D;
        public float targetRotation;
    }

    // List of body parts
    public List<BodyPart> bodyParts = new List<BodyPart>();

    public bool stabTest = false;

    private void Start()
    {
        movement = GetComponent<MovementPlayer>();
    }

    void OnEnable()
    {
        currentUpwardForce = 0;
        if (increaseForceCoroutine != null)
        {
            StopCoroutine(increaseForceCoroutine);
        }
        increaseForceCoroutine = StartCoroutine(IncreaseUpwardForce());
    }

    void OnDisable()
    {
        if (increaseForceCoroutine != null)
        {
            StopCoroutine(increaseForceCoroutine);
        }
    }

    IEnumerator IncreaseUpwardForce()
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            currentUpwardForce = Mathf.Lerp(0, upwardForce, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        currentUpwardForce = upwardForce; // ensure we end exactly at the target value
    }

    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        MoveRotationLimbs();
        if (movement.noMovement && stabTest && movement.stabilizationCooldown == 0 && movement.grounded)
        {
            ApplyStabilizingForce();
        }
    }

    void MoveRotationLimbs()
    {
        foreach (BodyPart part in bodyParts)
        {
            part.rigidbody2D.MoveRotation(part.targetRotation);
        }
    }

    void HandleRotationInput()
    {
        float rotationAdjustment = changeRate * Time.deltaTime;

        if (Input.GetKey(KeyCode.A))
        {
            freeze = false;
            AdjustRotationForAllParts(rotationAdjustment);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            freeze = false;
            AdjustRotationForAllParts(-rotationAdjustment);
        }
        else
        {
            freeze = true;
        }
    }

    void AdjustRotationForAllParts(float adjustment)
    {
        foreach (BodyPart part in bodyParts)
        {
            part.targetRotation += adjustment;
            part.targetRotation = Mathf.Clamp(part.targetRotation, minRotation, maxRotation);
        }
    }


    void ApplyStabilizingForce()
    {
        foreach (BodyPart bodyPart in bodyParts)
        {
            Vector2 currentVelocity = bodyPart.rigidbody2D.velocity;

            // Only apply force if there's significant movement
            if (currentVelocity.magnitude > 0.05f)
            {
                bodyPart.rigidbody2D.AddForce(-currentVelocity * forceMagnitude);
            }
        }
    }

}
