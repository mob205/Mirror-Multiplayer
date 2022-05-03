using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashmasterUpgrade : Upgrade
{
    public string layerName;
    public override void Initialize()
    {
        GetComponent<DashAbility>().OnAbilityCast += ChangeLayer;
    }
    private void ChangeLayer(AbilityUpgrade ability)
    {
        gameObject.layer = LayerMask.NameToLayer(layerName);
    }
}
