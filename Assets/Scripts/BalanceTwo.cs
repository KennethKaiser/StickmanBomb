using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalanceTwo : MonoBehaviour
{
    [SerializeField]
    private float targetRotation = 0;
    [SerializeField]
    private float adjustmentSpeed = 1.0f;

    public Rigidbody2D rb;
    private Rigidbody2D[] limbs;

    void Start()
    {
        Rigidbody2D[] all = GetComponentsInChildren<Rigidbody2D>();

        limbs = new Rigidbody2D[all.Length - 1];
        int i = 0;
        foreach(Rigidbody2D rigidbody2D in all)
        {
            if(rb != rigidbody2D)
            {
                limbs[i] = rigidbody2D;
                i++;
            }
        }
    }

    void Update()
    {
        float step = adjustmentSpeed * Time.deltaTime;
        float newRotation = Mathf.LerpAngle(rb.rotation, targetRotation, step);
        rb.MoveRotation(newRotation);
    }
}
