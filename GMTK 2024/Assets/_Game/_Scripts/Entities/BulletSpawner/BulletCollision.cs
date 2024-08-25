using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCollision : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer != 3 && collision.gameObject.layer != 6 && collision.gameObject.layer != 24)
            Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer != 3 && collision.gameObject.layer != 6 && collision.gameObject.layer != 24) 
            Destroy(gameObject);
    }
}
