using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirerateUpgrade : Upgrade
{
    public float firerateModifier;
    protected WeaponController weapon;
    public override void Initialize()
    {
        weapon = GetComponent<PlayerCombat>().Weapon;
        weapon.ModifyFirerate(firerateModifier);
    }
}
