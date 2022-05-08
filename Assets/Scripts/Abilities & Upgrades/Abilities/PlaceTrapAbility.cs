using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceTrapAbility : AbilityUpgrade
{
    [Header("Prefab")]
    public TrapItem trapPrefab;
    public float trapLifetime;
    public override void CastAbility(Vector2 mousePos)
    {
        var trap = Instantiate(trapPrefab, transform.position, Quaternion.identity);
        trap.caster = gameObject;
        trap.itemLifetime = trapLifetime;
        NetworkServer.Spawn(trap.gameObject);
        base.CastAbility(mousePos);
    }
}
