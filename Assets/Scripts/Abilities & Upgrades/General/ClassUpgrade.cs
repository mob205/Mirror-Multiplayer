using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ClassUpgrade : Upgrade
{
    public GameObject weapon;
    public Sprite sprite;
    public AbilityUpgrade classAbility;
    public float healthModifier = 1;

    public override void Initialize()
    {
        GetComponent<PlayerCombat>().SetWeapon(weapon);
        GetComponent<SpriteRenderer>().sprite = sprite;

        var health = GetComponent<Health>();
        health.MaxHealth *= healthModifier;
        if (NetworkServer.active)
        {
            health.Damage(-health.MaxHealth, null);
        }
        if (classAbility)
        {
            GetComponent<PlayerUpgrades>().AddAbilityUpgrade(classAbility);
        }
    }
    //private void Start()
    //{
    //    GetComponent<PlayerCombat>().SetWeapon(weapon);
    //    GetComponent<SpriteRenderer>().sprite = sprite;

    //    var health = GetComponent<Health>();
    //    health.MaxHealth *= healthModifier;
    //    if (NetworkServer.active)
    //    {
    //        health.Damage(-health.MaxHealth, null);
    //    }
    //    if (classAbility)
    //    {
    //        GetComponent<PlayerUpgrades>().AddAbilityUpgrade(classAbility);
    //    }
    //}
}
