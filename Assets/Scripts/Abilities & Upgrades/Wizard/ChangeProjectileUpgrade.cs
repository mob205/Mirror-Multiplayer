using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeProjectileUpgrade : Upgrade
{
    public Bullet newBullet;

    public override void Initialize()
    {
        var weapon = (ProjectileWeapon) GetComponent<PlayerCombat>().Weapon;
        weapon.ChangeBullet(newBullet);
    }
}