using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiercingBulletUpgrade : Upgrade
{
    public LayerMask ignoredLayers;
    private ProjectileWeapon weapon;
    public override void Initialize()
    {
        weapon = GetComponentInChildren<ProjectileWeapon>();
        weapon.OnShootEffects += ApplyUpgrade;
    }
    private void ApplyUpgrade(Bullet bullet)
    {
        bullet.collisionMask ^= ignoredLayers;
    }
}
