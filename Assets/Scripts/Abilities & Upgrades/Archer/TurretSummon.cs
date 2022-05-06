using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretSummon : NetworkBehaviour
{
    [Header("Targeting")]
    public float range;
    public LayerMask targetLayers;
    public float retargetDelay = 1;
    public Transform weaponPivot;

    [Header("Shooting")]
    public float attackSpeed;
    public float damage;
    public float bulletSpeed;
    public Bullet bulletPrefab;
    public Transform bulletSpawn;

    [HideInInspector] [SyncVar] public GameObject caster;

    private bool canFire = true;
    private GameObject currentTarget;

    public override void OnStartServer()
    {
        StartCoroutine(DelayedFindTarget());

    }
    private void Start()
    {
        var casterWeapon = caster.GetComponentInChildren<ProjectileWeapon>();
        if (casterWeapon)
        {
            casterWeapon.OnShoot += OnCasterShoot;
        }
    }
    private void Update()
    {
        if(NetworkServer.active && currentTarget)
        {
            var target = currentTarget.transform.position;
            RpcRotate(target);
            if (canFire)
            {
                Fire(target);
                RpcFire(target);
                StartCoroutine(ToggleFire());
            }
        }
    }
    private void OnCasterShoot(Bullet bullet)
    {
        Physics2D.IgnoreCollision(bullet.GetComponent<BoxCollider2D>(), GetComponent<BoxCollider2D>());
    }
    [ClientRpc]
    private void RpcFire(Vector2 target)
    {
        Fire(target);
    }
    [ClientRpc]
    private void RpcRotate(Vector2 target)
    {
        var dir = Utility.GetDirection(target, transform);
        weaponPivot.rotation = dir;
    }
    private void Fire(Vector2 target)
    {
        var dir = Utility.GetDirection(target, transform);

        var bullet = Instantiate(bulletPrefab, bulletSpawn.position, dir);
        var bulletComponent = bullet.GetComponent<Bullet>();

        bullet.GetComponent<Rigidbody2D>().velocity = bullet.transform.right * bulletSpeed;

        bulletComponent.Shooter = caster;
        bulletComponent.Damage = damage;
        bulletComponent.Weapon = caster.GetComponent<PlayerCombat>().Weapon;

        Physics2D.IgnoreCollision(bullet.GetComponent<BoxCollider2D>(), GetComponent<BoxCollider2D>());

    }
    private void FindNearestTarget()
    {
        currentTarget = null;
        var targetsInRange = Physics2D.OverlapCircleAll(transform.position, range);
        float leastDistance = Mathf.Infinity;
        foreach (var target in targetsInRange)
        {
            var squaredDist = (target.gameObject.transform.position - transform.position).sqrMagnitude;
            if (target.gameObject != gameObject && squaredDist < leastDistance && (targetLayers.value & (1 << (target.gameObject.layer))) > 0 && target.gameObject != caster)
            {
                currentTarget = target.gameObject;
                leastDistance = squaredDist;
            }
        }
    }
    private IEnumerator DelayedFindTarget()
    {
        while (true)
        {
            FindNearestTarget();
            yield return new WaitForSeconds(retargetDelay);
        }
    }
    private IEnumerator ToggleFire()
    {
        canFire = false;
        yield return new WaitForSeconds(1 / attackSpeed);
        canFire = true;
    }
}
