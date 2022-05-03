using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashmasterUpgrade : Upgrade
{
    public string layerName;
    public override void Initialize()
    {
        GetComponent<DashAbilityUpgrade>().OnAbilityCast += ChangeLayer;
    }
    private void ChangeLayer(AbilityUpgrade ability)
    {
        gameObject.layer = LayerMask.NameToLayer(layerName);
    }
}
