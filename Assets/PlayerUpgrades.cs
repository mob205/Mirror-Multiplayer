using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUpgrades : NetworkBehaviour
{
    [Command]
    public void CmdAddUpgrades(string[] upgrades)
    {
        RpcAddUpgrades(upgrades);
    }
    [ClientRpc]
    private void RpcAddUpgrades(string[] upgrades)
    {
        foreach(var upgrade in upgrades)
        {
            Debug.Log($"I have {upgrade}");
        }
    }
}
