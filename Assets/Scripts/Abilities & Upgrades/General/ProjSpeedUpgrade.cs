using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjSpeedUpgrade : MonoBehaviour
{
    public float projectileSpeedModifier;
    void Start()
    {
        var weapon = (ProjectileWeapon) GetComponent<PlayerCombat>().Weapon;
        weapon.ModifyBulletSpeed(projectileSpeedModifier);
    }
}
