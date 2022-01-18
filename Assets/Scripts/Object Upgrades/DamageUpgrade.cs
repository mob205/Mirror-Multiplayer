using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageUpgrade : MonoBehaviour
{
    public float damageModifier;
    private void Start()
    {
        Debug.Log("Applying damage upgrade.");
        var weapon = GetComponentInChildren<WeaponController>();
        if (weapon)
        {
            weapon.ModifyDamage(damageModifier);
        }
    }
}
