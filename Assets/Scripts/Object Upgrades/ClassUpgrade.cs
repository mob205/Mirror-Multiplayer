using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassUpgrade : MonoBehaviour
{
    public GameObject weapon;
    public Sprite sprite;
    public AbilityUpgrade classAbility;
    private void Start()
    {
        GetComponent<PlayerCombat>().SetWeapon(weapon);
        GetComponent<SpriteRenderer>().sprite = sprite;

        if (classAbility)
        {
            GetComponent<PlayerUpgrades>().AddAbilityUpgrade(classAbility);
        }
    }
}
