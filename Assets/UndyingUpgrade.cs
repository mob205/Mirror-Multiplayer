using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UndyingUpgrade : Upgrade
{
    public float healthModifier;
    public float healthOnHit;

    private Health health;
    public override void Initialize()
    {
        health = GetComponent<Health>();
        health.MaxHealth *= healthModifier;
        if (NetworkServer.active)
        {
            health.Damage(-health.MaxHealth, null);
        }
        GetComponentInChildren<MeleeHitbox>().OnHit += OnHit;
    }
    private void OnHit(Health target)
    {
        health.Damage(-healthOnHit, null);
    }
}
