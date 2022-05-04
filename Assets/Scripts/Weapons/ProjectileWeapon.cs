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

    public override bool ServerFire(Vector3 target, ref GameObject go)
    {
        if (!canFire) { return false; }
        // Getting the direction client-side will result in the shot missing if client is moving and shooting. 
        var dir = Utility.GetDirection(target, transform);

        // Spawn the bullet and set its velocity server-side
        GameObject bulletGO = Instantiate(bulletPrefab, transform.position, dir);
        var bulletComponent = ShootBullet(target, bulletGO);

        var identity = GetComponentInParent<NetworkIdentity>();
        if (!(identity.isServer && identity.isClient))
        {
            // Exclude hosts so event is not triggered twice
            OnShoot?.Invoke(bulletComponent);
        }

        NetworkServer.Spawn(bulletGO); 

        StartCoroutine(DelayedDestroy(bulletGO, bulletLifetime));
        StartCoroutine(ToggleFire());

        go = bulletGO;
        return true;
    }
    public override void SimulateFire(GameObject bullet, Vector3 target)
    {
        var bulletComponent = ShootBullet(target, bullet);
        OnShoot?.Invoke(bulletComponent);
    }
    private Bullet ShootBullet(Vector3 target, GameObject bullet)
    {
        var bulletComponent = bullet.GetComponent<Bullet>();
        bullet.transform.SetPositionAndRotation(transform.position, Utility.GetDirection(target, transform));
        bullet.GetComponent<Rigidbody2D>().velocity = bullet.gameObject.transform.right * bulletSpeed;
        bulletComponent.Shooter = transform.parent.gameObject;
        bulletComponent.Weapon = this;
        bulletComponent.Damage = damage;
        return bulletComponent;
    }
    private IEnumerator DelayedDestroy(GameObject go, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (go)
        {
            NetworkServer.Destroy(go);
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
