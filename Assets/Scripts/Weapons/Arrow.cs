using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : Bullet
{
    //protected override void OnTriggerEnter2D(Collider2D collision)
    //{
    //    var colGO = collision.gameObject;
    //    var target = colGO.GetComponent<Health>();

    //    //// Swords block arrow check
    //    //if (collision.CompareTag("Blocking"))
    //    //{
    //    //    NetworkServer.Destroy(gameObject);
    //    //}
    //    // Bullets should only damage on the server.
    //    if (isServer && target && colGO != Shooter && colGO.transform.parent.gameObject != Shooter)
    //    {
    //        target.Damage(Damage, Shooter);
    //        StickArrow(colGO);
    //    }
    //    // Visually stick to terrain and player objects on clients.
    //    else if ((collisionMask.value & (1 << (colGO.layer))) > 0 && colGO != Shooter)
    //    {
    //        StickArrow(colGO);
    //    }
    //}
    protected override void OnHitPlayer(Health target)
    {
        base.OnHitPlayer(target);
        StickArrow(target.gameObject);
    }
    protected override void OnHitWorld(GameObject hitGO)
    {
        StickArrow(hitGO);
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
