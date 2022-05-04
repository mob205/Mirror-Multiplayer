using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootProjectileAbility : AbilityUpgrade
{
    [Header("Projectile")]
    public RampupBullet bulletPrefab;
    public float baseDamage;
    public float bulletSpeed;
    public float damageGainRate;
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
        var bullet = Instantiate(bulletPrefab, transform.position, dir);

        bullet.transform.SetPositionAndRotation(transform.position, dir);
        bullet.GetComponent<Rigidbody2D>().velocity = bullet.transform.right * bulletSpeed;
        bullet.Shooter = transform.gameObject;
        bullet.Damage = baseDamage;
        bullet.DamageGainRate = damageGainRate;
    }
}
