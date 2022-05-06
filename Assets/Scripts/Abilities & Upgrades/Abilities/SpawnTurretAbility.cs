using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTurretAbility : AbilityUpgrade
{
    [Header("Prefab")]
    public TurretSummon turretPrefab;
    public override void CastAbility(Vector2 mousePos)
    {
        var turret = Instantiate(turretPrefab, transform.position, Quaternion.identity);
        turret.caster = gameObject;
        NetworkServer.Spawn(turret.gameObject);
        base.CastAbility(mousePos);
    }
    public void ChangePrefab(TurretSummon newPrefab)
    {
        turretPrefab = newPrefab;
    }
}
