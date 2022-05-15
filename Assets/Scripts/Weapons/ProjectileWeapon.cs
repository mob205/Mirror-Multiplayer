using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileWeapon : WeaponController
{
    public float bulletSpeed = 5;
    public Bullet weaponBulletPrefab;
    public float bulletLifetime = 2;

    public event Action<Bullet> OnShoot;
    public event Action<Bullet> OnShootEffects;

    public override bool ServerFire(Vector3 target)
    {
        if (!canFire) { return false; }

        ShootWeaponBullet(target);

        return true;
    }
    public override void SimulateFire(Vector3 target)
    {
        if (!netIdentity.isHost)
        {
            ShootWeaponBullet(target);
        }
    }
    private Bullet ShootWeaponBullet(Vector3 target)
    {
        var dir = Utility.GetDirection(target, transform);
        var bullet = ShootBullet(weaponBulletPrefab, dir, bulletSpeed, damage, bulletLifetime, true);
        StartCoroutine(ToggleFire());
        return bullet;
    }
    public Bullet ShootBullet(Bullet bulletPrefab, Quaternion dir, float bulletSpeed, float damage, float bulletLifetime, bool triggerEffects)
    {
        var bullet = Instantiate(bulletPrefab, transform.position, dir);
        var bulletComponent = bullet.GetComponent<Bullet>();

        bullet.GetComponent<Rigidbody2D>().velocity = bullet.transform.right * bulletSpeed;

        bulletComponent.Shooter = transform.parent.gameObject;
        bulletComponent.Weapon = this;
        bulletComponent.Damage = damage;

        OnShoot?.Invoke(bulletComponent);
        if (triggerEffects)
        {
            OnShootEffects?.Invoke(bulletComponent);
        }

        StartCoroutine(DelayedDestroy(bullet.gameObject, bulletLifetime));

        return bulletComponent;
    }
    private IEnumerator DelayedDestroy(GameObject go, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (go)
        {
            Destroy(go);
        }
    }
    public void SetBulletLifetime(float lifetime)
    {
        bulletLifetime = lifetime;
    }
}
