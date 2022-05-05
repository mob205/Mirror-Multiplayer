using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : Bullet
{
    protected override void OnPlayerHit(Health target)
    {
        base.OnPlayerHit(target);
        StickArrow(target.gameObject);
    }
    protected override void OnCollisionHit(GameObject hitGO)
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
