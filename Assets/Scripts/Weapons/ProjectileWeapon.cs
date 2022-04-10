using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileWeapon : WeaponController
{
    [SerializeField] private float bulletSpeed = 5;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletLifetime = 2;

    public override bool ServerFire(Vector3 target, ref GameObject go)
    {
        if (!canFire) { return false; }
        // Getting the direction client-side will result in the shot missing if client is moving and shooting. 
        var dir = Utility.GetDirection(target, transform);

        // Spawn the bullet and set its velocity server-side
        GameObject bulletGO = Instantiate(bulletPrefab, transform.position, dir);
        bulletGO.GetComponent<Rigidbody2D>().velocity = bulletGO.gameObject.transform.right * bulletSpeed;

        var bullet = bulletGO.GetComponent<Bullet>();
        bullet.Shooter = transform.parent.gameObject;
        bullet.Damage = damage;

        NetworkServer.Spawn(bulletGO); 

        StartCoroutine(DelayedDestroy(bulletGO, bulletLifetime));
        StartCoroutine(ToggleFire());

        go = bullet.gameObject;
        return true;
    }
    public override void SimulateFire(GameObject bullet, Vector3 target)
    {
        bullet.transform.position = transform.position;
        bullet.transform.rotation = Utility.GetDirection(target, transform);
        bullet.GetComponent<Rigidbody2D>().velocity = bullet.gameObject.transform.right * bulletSpeed;
        bullet.GetComponent<Bullet>().Shooter = transform.parent.gameObject;
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
