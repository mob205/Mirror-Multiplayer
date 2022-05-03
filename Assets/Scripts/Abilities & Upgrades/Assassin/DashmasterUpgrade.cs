using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashmasterUpgrade : Upgrade
{
    public int charges;
    public float castDelay;
    public string dashLayerName;
    public override void Initialize()
    {
        var ability = GetComponent<DashAbility>();
        ability.OnAbilityCast += ChangeLayer;
        ability.maxCharges = charges;
        ability.baseCastDelay = castDelay;
        ability.ResetCharges();
    }
    private void ChangeLayer(AbilityUpgrade ability)
    {
        gameObject.layer = LayerMask.NameToLayer(dashLayerName);
    }
}
