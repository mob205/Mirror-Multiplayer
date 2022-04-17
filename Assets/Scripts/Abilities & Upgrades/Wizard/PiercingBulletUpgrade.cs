using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiercingBulletUpgrade : MonoBehaviour
{
    public LayerMask ignoredLayers;
    private ProjectileWeapon weapon;
    void Start()
    {
        weapon = GetComponentInChildren<ProjectileWeapon>();
        weapon.OnShoot += ApplyUpgrade;
    }
    private void ApplyUpgrade(Bullet bullet)
    {
        bullet.collisionMask ^= ignoredLayers;
    }
}
