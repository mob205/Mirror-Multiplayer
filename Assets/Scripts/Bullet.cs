using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Bullet : NetworkBehaviour
{
    public GameObject Shooter { get; set; }
    public float Damage { get; set; }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var target = collision.gameObject.GetComponent<Health>();
        if (isServer && target && collision.gameObject != Shooter)
        {
            target.Damage(Damage);
            NetworkServer.Destroy(gameObject);
        }
    }
}