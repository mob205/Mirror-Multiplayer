using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Bullet : NetworkBehaviour
{
    [SerializeField] protected LayerMask collisionMask;
    public GameObject Shooter { get; set; }
    public float Damage { get; set; }


    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        var colGO = collision.gameObject;

        if(colGO == Shooter) { return; }
        if(colGO.transform.parent && colGO.transform.parent.gameObject == Shooter) { return; }

        var targetHealth = colGO.GetComponent<Health>();
        if(isServer && targetHealth)
        {
            targetHealth.Damage(Damage, Shooter);
        }
        if((collisionMask.value & (1 << (colGO.layer))) > 0)
        {
            Destroy(gameObject);
        }
    }
}