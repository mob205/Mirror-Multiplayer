using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileWeapon : WeaponController
{
    [SerializeField] private float bulletSpeed = 5;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletLifetime = 2;

    public override void ClientFire()
    {
        //ServerFire(mainCam.ScreenToWorldPoint(Input.mousePosition));
    }
    public override GameObject ServerFire(Vector3 target)
    {
        if (!canFire) { return null; }
        Debug.Log("firing from wep controller");
        // Getting the direction client-side will result in the shot missing if client is moving and shooting. 
        var dir = GetDirection(target);

        // Spawn the bullet and set its velocity server-side
        GameObject bulletGO = Instantiate(bulletPrefab, transform.position, dir);
        bulletGO.GetComponent<Rigidbody2D>().velocity = bulletGO.gameObject.transform.right * bulletSpeed;

        var bullet = bulletGO.GetComponent<Bullet>();
        bullet.Shooter = transform.parent.gameObject;
        bullet.Damage = damage;

        // potentially unnecessary? SimulateFire could just create a new gameobject rather than requiring a reference
        NetworkServer.Spawn(bulletGO); 

        StartCoroutine(DelayedDestroy(bulletGO, bulletLifetime));
        StartCoroutine(ToggleFire());

        return bullet.gameObject;
    }
    public override void SimulateFire(GameObject bullet, Vector3 target)
    {
        Debug.Log("Simulating fire on clients");
        bullet.transform.position = transform.position;
        bullet.transform.rotation = GetDirection(target);
        bullet.GetComponent<Rigidbody2D>().velocity = bullet.gameObject.transform.right * bulletSpeed;
    }
    private Quaternion GetDirection(Vector3 target)
    {
        // Get displacement vector components from player object to target
        var y = target.y - transform.position.y;
        var x = target.x - transform.position.x;

        // Get rotation from the arctangent of displacement components
        float angle = Mathf.Atan2(y, x) * Mathf.Rad2Deg;
        return Quaternion.Euler(new Vector3(0, 0, angle));
    }
    private IEnumerator DelayedDestroy(GameObject go, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (go)
        {
            NetworkServer.Destroy(go);
        }
    }
}
