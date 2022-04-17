using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ExplosionAbility : AbilityUpgrade
{
    public ParticleSystem particles;
    public LayerMask wallLayers;
    public float range;
    public float damage;
    public override void CastAbility(Vector2 mousePos)
    {
        var self = GetComponent<Health>();
        var targetsInRange = Physics2D.OverlapCircleAll(transform.position, range);
        foreach(var target in targetsInRange)
        {
            var targetHealth = target.GetComponent<Health>();
            if (targetHealth && targetHealth != self && !Physics2D.Linecast(transform.position, target.transform.position, wallLayers))
            {
                targetHealth.Damage(damage, gameObject);
            }
        }
        StartCooldown();
    }
    public override void ClientCastAbility(Vector2 mousePos)
    {
        Instantiate(particles, transform.position, Quaternion.identity);
        base.ClientCastAbility(mousePos);
    }
}
