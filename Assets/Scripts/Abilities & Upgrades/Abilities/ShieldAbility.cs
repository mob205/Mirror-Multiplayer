using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldAbility : AbilityUpgrade
{
    [Header("Gameplay")]
    public GameObject shieldPrefab;
    public float castDistance;
    public float shieldDuration;

    public override void CastAbility(Vector2 mousePos)
    {
        SpawnShield(mousePos);
        base.CastAbility(mousePos);
    }
    public override void ClientCastAbility(Vector2 mousePos)
    {
        if (identity.isClientOnly)
        {
            SpawnShield(mousePos);
        }
        base.ClientCastAbility(mousePos);
    }
    private void SpawnShield(Vector2 mousePos)
    {
        var rot = Utility.GetDirection(mousePos, transform);
        var dir = (Vector3) mousePos - transform.position;
        var shield = Instantiate(shieldPrefab, transform.position + (dir.normalized * castDistance), rot, transform);
        Destroy(shield, shieldDuration);
    }
}
