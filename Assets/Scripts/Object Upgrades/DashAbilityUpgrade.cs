using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashAbilityUpgrade : AbilityUpgrade
{
    protected override void CastAbility()
    {
        Debug.Log("Casting ability");
        StartCooldown();
    }
}
