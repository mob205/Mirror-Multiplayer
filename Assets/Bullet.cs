using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Bullet : NetworkBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform != transform.parent && collision.gameObject.GetComponent<PlayerCombat>())
        {
            Debug.Log("Collided with a player.");
            NetworkServer.Destroy(gameObject);
        }
    }
}
