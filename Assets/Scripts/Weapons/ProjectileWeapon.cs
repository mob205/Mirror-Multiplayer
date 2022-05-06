using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileWeapon : WeaponController
{
    [SerializeField] private float bulletSpeed = 5;
    [SerializeField] private Bullet weaponBulletPrefab;
    [SerializeField] private float bulletLifetime = 2;

    public event Action<Bullet> OnShoot;

    public override bool ServerFire(Vector3 target)
    {
        if (!canFire) { return false; }

        if (!playerIdentity.isHost)
        {
            var bullet = ShootWeaponBullet(target);

            // Exclude hosts so event is not triggered twice
            OnShoot?.Invoke(bullet);
        }

        StartCoroutine(ToggleFire());

        return true;
    }
    public override void SimulateFire(Vector3 target)
    {
        var dir = Utility.GetDirection(target, transform);
        var bullet = ShootWeaponBullet(target);

        OnShoot?.Invoke(bullet);

        StartCoroutine(ToggleFire());
    }
    private Bullet ShootWeaponBullet(Vector3 target)
    {
        var dir = Utility.GetDirection(target, transform);
        var bullet = ShootBullet(weaponBulletPrefab, dir);
        return bullet;
    }
    public Bullet ShootBullet(Bullet bulletPrefab, Quaternion dir)
    {
        var bullet = Instantiate(bulletPrefab, transform.position, dir);
        var bulletComponent = bullet.GetComponent<Bullet>();

        bullet.GetComponent<Rigidbody2D>().velocity = bullet.transform.right * bulletSpeed;

        bulletComponent.Shooter = transform.parent.gameObject;
        bulletComponent.Weapon = this;
        bulletComponent.Damage = damage;

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
    public void ChangeBullet(Bullet newPrefab)
    {
        weaponBulletPrefab = newPrefab;
    }
    public void ModifyBulletSpeed(float modifier)
    {
        bulletSpeed *= modifier;
    }
    public void SetBulletLifetime(float lifetime)
    {
        bulletLifetime = lifetime;
    }
}
