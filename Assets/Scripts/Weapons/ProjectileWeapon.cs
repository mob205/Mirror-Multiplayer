using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileWeapon : WeaponController
{
    [SerializeField] private float bulletSpeed = 5;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletLifetime = 2;

    public event Action<Bullet> OnShoot;

    public override bool ServerFire(Vector3 target)
    {
        if (!canFire) { return false; }
        var bullet = ShootBullet(target);

        if (!playerIdentity.isHost)
        {
            // Exclude hosts so event is not triggered twice
            OnShoot?.Invoke(bullet);
        }
        return true;
    }
    public override void SimulateFire(Vector3 target)
    {
        var bullet = ShootBullet(target);
        OnShoot?.Invoke(bullet);
    }
    private Bullet ShootBullet(Vector3 target)
    {
        var dir = Utility.GetDirection(target, transform);
        var bullet = Instantiate(bulletPrefab, transform.position, dir);
        var bulletComponent = bullet.GetComponent<Bullet>();

        bullet.transform.SetPositionAndRotation(transform.position, Utility.GetDirection(target, transform));
        bullet.GetComponent<Rigidbody2D>().velocity = bullet.transform.right * bulletSpeed;

        bulletComponent.Shooter = transform.parent.gameObject;
        bulletComponent.Weapon = this;
        bulletComponent.Damage = damage;

        StartCoroutine(DelayedDestroy(bullet.gameObject, bulletLifetime));
        StartCoroutine(ToggleFire());

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
        bulletPrefab = newPrefab.gameObject;
    }
    public void ModifyBulletSpeed(float modifier)
    {
        bulletSpeed *= modifier;
    }
}
