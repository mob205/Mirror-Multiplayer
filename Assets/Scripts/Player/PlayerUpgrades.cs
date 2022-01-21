using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUpgrades : NetworkBehaviour
{
    [SerializeField] MonoBehaviour testAbility;
    private void Start()
    {
        // The local copy of every player object requests and applies its upgrades on start.
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
        if (testAbility)
        {
            CopyComponent(testAbility, gameObject);
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
