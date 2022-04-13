using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeProjectileUpgrade : MonoBehaviour
{
    public Bullet newBullet;

    void Start()
    {
        var weapon = (ProjectileWeapon) GetComponent<PlayerCombat>().Weapon;
        weapon.ChangeBullet(newBullet);
    }
}