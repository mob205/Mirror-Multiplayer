using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ClassUpgrade : MonoBehaviour
{
    public GameObject weapon;
    public Sprite sprite;
    public AbilityUpgrade classAbility;
    public float healthModifier = 1;
    private void Start()
    {
        GetComponent<PlayerCombat>().SetWeapon(weapon);
        GetComponent<SpriteRenderer>().sprite = sprite;

        if (NetworkServer.active)
        {
            var health = GetComponent<Health>();
            health.MaxHealth *= healthModifier;
            health.Damage(-(health.MaxHealth), null);
        }

        if (classAbility)
        {
            GetComponent<PlayerUpgrades>().AddAbilityUpgrade(classAbility);
        }
    }
}
