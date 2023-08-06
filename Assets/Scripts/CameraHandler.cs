using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    public Transform player;
    public float roofY; // Set this to the Y position of your roof
    public float moveUpAmount; // The amount you want to move the camera up

    void Update()
    {
        if (player.localPosition.y >= roofY)
        {
            Vector3 newPosition = transform.position;
            newPosition.y += moveUpAmount; // This will move the camera up by 'moveUpAmount'
            transform.position = newPosition;

            // Update the roofY value so the camera will move again when the player reaches this new height
            roofY += 20;
        }
        else if (player.localPosition.y < roofY - 20)
        {
            Vector3 newPosition = transform.position;
            newPosition.y -= moveUpAmount; // This will move the camera up by 'moveUpAmount'
            transform.position = newPosition;
            // Update the roofY value so the camera will move again when the player reaches this new height
            roofY -= 20;
        }
    }
}
