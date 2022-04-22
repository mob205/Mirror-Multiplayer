using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddAbilityUpgrade : Upgrade
{
    public AbilityUpgrade ability;
    public override void Initialize()
    {
        GetComponent<PlayerUpgrades>().AddAbilityUpgrade(ability);
    }
}
