using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balance : MonoBehaviour
{
    [SerializeField]
    private float targetRotation;
    [SerializeField]
    private float upwardForce;
    private Rigidbody2D rb;


    //Testing for enabling speed
    private float currentUpwardForce;
    private Coroutine increaseForceCoroutine;
    [SerializeField]
    private float duration = 1f;

    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
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
        rb.MoveRotation(Mathf.LerpAngle(rb.rotation, targetRotation, currentUpwardForce * Time.deltaTime));
    }


}
