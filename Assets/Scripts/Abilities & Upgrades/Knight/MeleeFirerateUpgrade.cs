using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeFirerateUpgrade : FirerateUpgrade
{
    public float swingspeedModifier;
    public override void Initialize()
    {
        base.Initialize();
        if(weapon is MeleeWeapon)
        {
            (weapon as MeleeWeapon).ModifySwingSpeed(swingspeedModifier);
        }
        else
        {
            Debug.LogError("Attempted to modify swing speed of a non-melee weapon");
        }
    }
}
