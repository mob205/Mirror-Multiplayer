using Mirror;
using System.Collections;
using UnityEngine;

public class PlayerCombat : NetworkBehaviour
{
    [SerializeField] private float fireRate = 1;
    [SerializeField] private float bulletSpeed = 5;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletLifetime = 2;
    [SerializeField] private float damage = 10;

    private bool canFire = true;
    private Camera mainCam;
    void Start()
    {
        mainCam = Camera.main;
    }

    private void Update()
    {
        // Check canFire and toggle canFire client-side to avoid latency. Possible cheating?
        if (canFire && hasAuthority && Input.GetButton("Fire1"))
        {
            CmdFire(GetDirection());
            StartCoroutine(ToggleFire());
        }
    }
    #region Shooting
    private Quaternion GetDirection()
    {
        // Get displacement vector components from player object to mouse
        var mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        var y = mousePos.y - transform.position.y;
        var x = mousePos.x - transform.position.x;
        
        // Get rotation from the arctangent of displacement components
        float angle = Mathf.Atan2(y, x) * Mathf.Rad2Deg;
        return Quaternion.Euler(new Vector3(0, 0, angle));
    }
    [Command]
    void CmdFire(Quaternion dir)
    {
        // Spawn the bullet and set its velocity server-side
        GameObject bulletGO = Instantiate(bulletPrefab, transform.position, dir);
        bulletGO.GetComponent<Rigidbody2D>().velocity = bulletGO.gameObject.transform.right * bulletSpeed;

        var bullet = bulletGO.GetComponent<Bullet>();
        bullet.Shooter = gameObject;
        bullet.Damage = damage;
        NetworkServer.Spawn(bulletGO);

        SimulateBullet(bulletGO);

        StartCoroutine(DelayedDestroy(bulletGO, bulletLifetime));
    }
    // Simulate the bullet client-side to avoid getting position from server-side bullet. 
    [ClientRpc]
    private void SimulateBullet(GameObject bullet)
    {
        bullet.GetComponent<Rigidbody2D>().velocity = bullet.gameObject.transform.right * bulletSpeed;
    }
    private IEnumerator ToggleFire()
    {
        canFire = false;
        yield return new WaitForSeconds(1/fireRate);
        canFire = true;
    }
    private IEnumerator DelayedDestroy(GameObject go, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (go)
        {
            NetworkServer.Destroy(go);
        }
    }
    #endregion
}
