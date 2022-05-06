using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifySummonUpgrade : Upgrade
{
    [Header("Prefab")]
    public TurretSummon newTurret;
    public override void Initialize()
    {
        var ability = GetComponent<SpawnTurretAbility>();
        ability.ChangePrefab(newTurret);
    }
}
