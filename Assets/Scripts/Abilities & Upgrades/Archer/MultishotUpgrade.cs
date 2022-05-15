using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultishotUpgrade : Upgrade
{
    public int extraBullets;
    public float degreeOffset;
    public float bulletLifetime;
    private ProjectileWeapon weapon;
    public override void Initialize()
    {
        weapon = GetComponentInChildren<ProjectileWeapon>();
        weapon.OnShootEffects += OnShoot;
        weapon.bulletLifetime = bulletLifetime;
    }
    private void OnShoot(Bullet bullet)
    {
        var baseAngle = bullet.transform.rotation.eulerAngles.z;
        float newAngle;
        int layer = 1;
        for(int i = 1; i <= extraBullets; i++)
        {
            if(i % 2 == 0)
            {
                newAngle = baseAngle + (degreeOffset * layer);
                layer++;
            }
            else
            {
                newAngle = baseAngle - (degreeOffset * layer);
            }
            var rot = Quaternion.Euler(new Vector3(0, 0, newAngle));
            weapon.ShootBullet(bullet, rot, weapon.bulletSpeed, bullet.Damage, weapon.bulletLifetime, false);
        }
    }
}
