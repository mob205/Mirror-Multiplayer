using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceTrapAbility : AbilityUpgrade
{
    public TrapItem trapPrefab;
    public override void CastAbility(Vector2 mousePos)
    {
        var trap = Instantiate(trapPrefab, transform.position, Quaternion.identity);
        trap.caster = gameObject;
        NetworkServer.Spawn(trap.gameObject);
        StartCooldown();
    }
}
