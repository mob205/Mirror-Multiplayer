using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddAbilityUpgrade : MonoBehaviour
{
    public AbilityUpgrade ability;
    private void Start()
    {
        GetComponent<PlayerUpgrades>().AddAbilityUpgrade(ability);
    }
}
