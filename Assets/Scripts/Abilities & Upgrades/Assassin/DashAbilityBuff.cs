using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashAbilityBuff : Upgrade
{
    public float damageModifier;
    public float cooldownModifier;
    public override void Initialize()
    {
        var ability = GetComponent<DashAbility>();
        if (ability)
        {
            ability.damage *= damageModifier;
            ability.baseCooldown *= cooldownModifier;
        }
    }
}
