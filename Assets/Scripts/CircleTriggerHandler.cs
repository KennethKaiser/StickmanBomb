using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleTriggerHandler : MonoBehaviour
{
    private bool _isLimbInside = false;
    public bool IsLimbInside => _isLimbInside;

    [SerializeField]
    private float _forceMultiplier;
    public float forceMultiplier => _forceMultiplier;

    private int _limbsInside = 0; // Counter for limbs currently inside the circle

    private void Update()
    {
        _isLimbInside = (_limbsInside > 0); // Update IsLimbInside based on the counter
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Limb"))
        {
            _limbsInside++; // Increment the counter when a limb enters
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Limb"))
        {
            _limbsInside = Mathf.Max(0, _limbsInside - 1); // Decrement the counter when a limb exits, ensuring it doesn't go below zero
        }
    }

    /*void OnGUI()
    {
        // Display IsLimbInside value at the top-left corner of the screen
        GUI.Label(new Rect(10, 10, 500, 20), "Is Limb Inside: " + _isLimbInside);
    }*/
}
