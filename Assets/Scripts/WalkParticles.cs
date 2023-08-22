using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkParticles : MonoBehaviour
{
    public GameObject dustParticlePrefab;
    private bool isOnGround = false;
    public LayerMask groundLayer;  // Assign this in the inspector by selecting the 'Ground' layer
    private float destroyTime;


    private void Start()
    {
        destroyTime = dustParticlePrefab.GetComponent<ParticleSystem>().main.duration;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isOnGround && (groundLayer.value & (1 << collision.gameObject.layer)) != 0)
        {
            foreach (ContactPoint2D contact in collision.contacts)
            {
                PlayDustEffect(contact.point);
            }
            isOnGround = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (isOnGround && (groundLayer.value & (1 << collision.gameObject.layer)) != 0)
        {
            isOnGround = false;
        }
    }

    private void PlayDustEffect(Vector2 position)
    {
        GameObject gameObjecter = Instantiate(dustParticlePrefab, position, Quaternion.identity);
        Destroy(gameObjecter, destroyTime);
    }
}
