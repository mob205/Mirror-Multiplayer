using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthUpgrade : Upgrade
{
    public float healthModifier;
    public override void Initialize()
    {
        var health = GetComponent<Health>();
        health.MaxHealth *= healthModifier;
        if (GetComponent<NetworkIdentity>().isServer)
        {
            health.Damage(-health.MaxHealth, null);
        }
    }
}
