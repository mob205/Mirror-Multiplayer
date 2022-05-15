using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootProjectileAbility : AbilityUpgrade
{
    [Header("Projectile")]
    public Bullet bulletPrefab;
    public float baseDamage;
    public float bulletSpeed;
    public float lifetime;
    public override void CastAbility(Vector2 mousePos)
    {
        ShootBullet(mousePos);
        base.CastAbility(mousePos);
    }
    public override void ClientCastAbility(Vector2 mousePos)
    {
        if (!identity.isHost)
        {
            ShootBullet(mousePos);
        }
        base.ClientCastAbility(mousePos);
    }
    private void ShootBullet(Vector3 mousePos)
    {
        var dir = Utility.GetDirection(mousePos, transform);
        GetComponentInChildren<ProjectileWeapon>().ShootBullet(bulletPrefab, dir, bulletSpeed, baseDamage, lifetime, true);
    }
}
