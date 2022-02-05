using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Bullet : NetworkBehaviour
{
    [SerializeField] private LayerMask collisionMask;
    public GameObject Shooter { get; set; }
    public float Damage { get; set; }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        var colGO = collision.gameObject;
        var target = colGO.GetComponent<Health>();

        // Swords block arrow check
        if (collision.CompareTag("Blocking"))
        {
            NetworkServer.Destroy(gameObject);
        }
        // Bullets should only damage on the server.
        if (isServer && target && colGO != Shooter)
        {
            target.Damage(Damage, Shooter);
            StickArrow(colGO);
        }
        // Visually stick to terrain and player objects on clients.
        else if ((collisionMask.value & (1 << (colGO.layer))) > 0 && colGO != Shooter)
        {
            StickArrow(colGO);
        }
    }
    private void StickArrow(GameObject target)
    {
        var rb = GetComponent<Rigidbody2D>();
        rb.velocity = Vector2.zero;
        transform.parent = target.transform;
        // Remove this script after sticking so it does not continue to register collisions
        rb.isKinematic = true;
        Destroy(this);
    }
}