using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Bullet : NetworkBehaviour
{
    public LayerMask collisionMask;
    public GameObject Shooter { get; set; }
    public WeaponController Weapon { get; set; }
    public float Damage { get; set; }


    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        var colGO = collision.gameObject;

        if(colGO == Shooter) { return; }
        if(colGO.transform.parent && colGO.transform.parent.gameObject == Shooter) { return; }

        var targetHealth = colGO.GetComponent<Health>();
        if(NetworkServer.active && targetHealth)
        {
            OnPlayerHit(targetHealth);
        }
        if((collisionMask.value & (1 << (colGO.layer))) > 0)
        {
            OnCollisionHit(colGO);
        }
    }
    protected virtual void OnPlayerHit(Health target)
    {
        target.Damage(Damage, Shooter);
        if (Weapon)
        {
            Weapon.TriggerHit(target);
        }
    }
    protected virtual void OnCollisionHit(GameObject hitGO)
    {
        Destroy(gameObject);
    }
}