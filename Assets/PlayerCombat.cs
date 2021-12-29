using Mirror;
using System.Collections;
using UnityEngine;

public class PlayerCombat : NetworkBehaviour
{
    [SerializeField] private float fireRate = 1;
    [SerializeField] private float bulletSpeed = 5;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletLifetime = 2;

    private bool canFire = true;
    private Camera mainCam;
    void Start()
    {
        mainCam = Camera.main;
    }

    private void Update()
    {
        if (canFire && hasAuthority && Input.GetButton("Fire1"))
        {
            CmdFire(GetDirection());
            StartCoroutine(ToggleFire());
        }
    }
    private Quaternion GetDirection()
    {
        var mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 dist = new Vector2(mousePos.x - transform.position.x, mousePos.y - transform.position.y);
        float angle = Mathf.Atan2(dist.y, dist.x) * Mathf.Rad2Deg;
        return Quaternion.Euler(new Vector3(0, 0, angle));
    }
    [Command]
    void CmdFire(Quaternion dir)
    {
        GameObject bullet = Instantiate(bulletPrefab, transform.position, dir);
        bullet.GetComponent<Rigidbody2D>().velocity = bullet.gameObject.transform.right * bulletSpeed;
        NetworkServer.Spawn(bullet);
        SimulateBullet(bullet);

        StartCoroutine(DelayedDestroy(bullet, bulletLifetime));
    }
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
        if (go)
        {
            yield return new WaitForSeconds(delay);
            Destroy(go);
            NetworkServer.UnSpawn(go);
        }
    }
}
