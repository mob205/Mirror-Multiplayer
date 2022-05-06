using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;

public class PlayerHealth : Health
{
    public static event Action<PlayerHealth, uint> OnPlayerDeath;

    protected override void DoDeathSequence(uint killerID)
    {
        gameObject.SetActive(false);
        OnPlayerDeath?.Invoke(this, killerID);
        RpcStartDeath(killerID);
    }
    [ClientRpc]
    private void RpcStartDeath(uint killerID)
    {
        gameObject.SetActive(false);
    }
}
