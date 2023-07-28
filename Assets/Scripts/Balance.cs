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
    

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        rb.MoveRotation(Mathf.LerpAngle(rb.rotation, targetRotation, upwardForce * Time.deltaTime));
    }
}
