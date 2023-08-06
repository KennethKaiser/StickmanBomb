using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnTheGround : MonoBehaviour
{

    public GameObject checker;
    private bool grounded;
    [SerializeField]
    private float positionRadius;
    public LayerMask ground;

    public bool IsGrounded { get { return grounded; } }

    void Update()
    {
        grounded = Physics2D.OverlapCircle(checker.transform.position, positionRadius, ground);
    }

    void OnDrawGizmos()
    {
        if (enabled)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(checker.transform.position, positionRadius);
        }
        
    }
}
