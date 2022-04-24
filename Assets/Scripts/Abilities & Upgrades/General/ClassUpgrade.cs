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
    public List<Upgrade> classUpgrades;

    public override void Initialize()
    {
        GetComponent<PlayerCombat>().SetWeapon(weapon);
        GetComponent<SpriteRenderer>().sprite = sprite;

        var playerUpgrades = GetComponent<PlayerUpgrades>();
        foreach(var upgrade in classUpgrades)
        {
            playerUpgrades.AddUpgrade(upgrade);
        }
    }
}
