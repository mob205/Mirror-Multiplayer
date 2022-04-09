using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashAbilityBuff : MonoBehaviour
{
    public float damageModifier;
    public float cooldownModifier;
    void Start()
    {
        var ability = GetComponent<DashAbilityUpgrade>();
        if (ability)
        {
            ability.damage *= damageModifier;
            ability.baseCooldown *= cooldownModifier;
        }
    }
}
