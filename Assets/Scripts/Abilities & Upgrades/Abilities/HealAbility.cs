using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealAbility : AbilityUpgrade
{
    public float healAmount;
    public override void CastAbility(Vector2 mousePos)
    {
        GetComponent<Health>().Damage(-healAmount, gameObject);
        StartCooldown();
    }
}
