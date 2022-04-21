using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageUpgrade : Upgrade
{
    public float damageModifier;
    public override void Initialize()
    {
        var weapon = GetComponentInChildren<WeaponController>();
        if (weapon)
        {
            weapon.ModifyDamage(damageModifier);
        }
    }
}
