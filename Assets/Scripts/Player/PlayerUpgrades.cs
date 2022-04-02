using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUpgrades : NetworkBehaviour
{
    [SerializeField] private MonoBehaviour defaultUpgrade;
    [SerializeField] private MonoBehaviour testUpgrade;

    private int abilityCount;
    private void Start()
    {
        // The local copy of every player object requests and applies its upgrades on start.
        CmdGetUpgrades(netId);
    }
    [Command(requiresAuthority = false)]
    public void CmdGetUpgrades(uint targetID, NetworkConnectionToClient sender = null)
    {
        var target = NetworkServer.spawned[targetID].connectionToClient;
        TargetAddUpgrades(sender, UpgradeManager.GetUpgradesForConn(target));
    }
    [TargetRpc]
    private void TargetAddUpgrades(NetworkConnection target, string[] upgradeIDs)
    {
        foreach(var upgradeID in upgradeIDs)
        {
            var upgrade = UpgradeManager.GetUpgradeFromID(upgradeID);
            CopyComponent(upgrade, gameObject);
        }
        if (upgradeIDs.Length == 0 && defaultUpgrade)
        {
            CopyComponent(defaultUpgrade, gameObject);
        }

        // Testing
        if (upgradeIDs.Length == 0 && testUpgrade)
        {
            CopyComponent(testUpgrade, gameObject);
        }
    }
    public void AddAbilityUpgrade(AbilityUpgrade ability)
    {
        AbilityUpgrade component = (AbilityUpgrade) CopyComponent(ability, gameObject);
        component.OrderNumber = abilityCount;
        abilityCount++;
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
