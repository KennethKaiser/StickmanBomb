using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoreColl : MonoBehaviour
{
    public Collider2D bomb;
    void Awake()
    {
        var colliders = GetComponentsInChildren<Collider2D>();
        for (int i = 0; i < colliders.Length; i++)
        {
            for (int k = i + 1; k < colliders.Length; k++)
            {
                Physics2D.IgnoreCollision(colliders[i], colliders[k]);
            }

            Physics2D.IgnoreCollision(colliders[i], bomb);
        }
    }

    
}
