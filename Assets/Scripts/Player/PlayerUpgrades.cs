using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUpgrades : NetworkBehaviour
{
    private void Start()
    {
        CmdGetUpgrades();
    }
    [Command(requiresAuthority = false)]
    public void CmdGetUpgrades(NetworkConnectionToClient conn = null)
    {
        TargetAddUpgrades(conn, UpgradeManager.GetUpgradesForConn(conn));
    }
    [TargetRpc]
    private void TargetAddUpgrades(NetworkConnection target, string[] upgradeIDs)
    {
        foreach(var upgradeID in upgradeIDs)
        {
            var upgrade = UpgradeManager.GetUpgradeFromID(upgradeID);
            CopyComponent(upgrade, gameObject);
        }
    }
    private Component CopyComponent(Component original, GameObject destination)
    {
        // Get and assign values from the prefab to the new component through reflection
        System.Type type = original.GetType();
        Component copy = destination.AddComponent(type);

        System.Reflection.FieldInfo[] fields = type.GetFields();
        foreach(System.Reflection.FieldInfo field in fields)
        {
            field.SetValue(copy, field.GetValue(original));
        }
        return copy;
    }
}
