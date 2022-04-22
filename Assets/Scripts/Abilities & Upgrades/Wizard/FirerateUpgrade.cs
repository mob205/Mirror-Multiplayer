using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirerateUpgrade : Upgrade
{
    public float firerateModifier;
    public override void Initialize()
    {
        GetComponent<PlayerCombat>().Weapon.ModifyFirerate(firerateModifier);
    }
}
