using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackstabHitbox : MeleeHitbox
{
    public float DamageModifier { get; set; }
    public float AngleThreshold { get; set; }
    protected override void DamageTarget(Health target)
    {
        var shooterAngle = GetComponentInParent<PlayerCombat>().FacingAngle;
        var targetAngle = target.GetComponent<PlayerCombat>().FacingAngle;
        var diff = targetAngle - shooterAngle;
        if(diff > 180)
        {
            diff -= 360;
        }
        if(Mathf.Abs(diff) < AngleThreshold)
        {
            base.DamageTarget(target, Damage * DamageModifier);
        }
        else
        {
            base.DamageTarget(target, Damage);
        }
    }
}
