using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjSpeedUpgrade : Upgrade
{
    public float projectileSpeedModifier;
    public override void Initialize()
    {
        var weapon = (ProjectileWeapon) GetComponent<PlayerCombat>().Weapon;
        weapon.bulletSpeed *= projectileSpeedModifier;
    }
}
