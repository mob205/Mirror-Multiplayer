using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjSpeedUpgrade : MonoBehaviour
{
    public float projectileSpeedModifier;
    void Start()
    {
        var weapon = (ProjectileWeapon) GetComponent<PlayerCombat>().Weapon;
        Debug.Log(GetComponent<PlayerCombat>().Weapon.name);
        Debug.Log(weapon.name);
        weapon.ModifyBulletSpeed(projectileSpeedModifier);
    }
}
