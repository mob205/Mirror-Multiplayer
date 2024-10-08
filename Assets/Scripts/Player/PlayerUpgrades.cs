using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUpgrades : NetworkBehaviour
{
    [SerializeField] private Upgrade defaultUpgrade;
    [SerializeField] private Upgrade testUpgrade;

    private int abilityCount;
    private void Start()
    {
        if(NetworkServer.active)
        {
            AddUpgrades(UpgradeManager.GetUpgradesForConn(NetworkServer.spawned[netId].connectionToClient));
        }
        else
        {
            // The local copy of every player object requests and applies its upgrades on start.
            CmdGetUpgrades(netId);
        }
        
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
        AddUpgrades(upgradeIDs);
    }
    private void AddUpgrades(string[] upgradeIDs)
    {
        foreach (var upgradeID in upgradeIDs)
        {
            var upgrade = UpgradeManager.GetUpgradeFromID(upgradeID);
            //CopyUpgrade(upgrade, gameObject).Initialize();

            //var copy = CopyUpgrade(upgrade, gameObject);
            //copy.Initialize();
            //Debug.Log($"Initialized {copy.GetType()} upgrade");
            AddUpgrade(upgrade);
        }
        if (upgradeIDs.Length == 0 && defaultUpgrade)
        {
            AddUpgrade(defaultUpgrade);
        }

        // Testing
        if (upgradeIDs.Length == 0 && testUpgrade)
        {
            AddUpgrade(testUpgrade);
        }
    }
    public void AddUpgrade(Upgrade upgrade)
    {
        var copy = CopyUpgrade(upgrade, gameObject);
        copy.Initialize();
        Debug.Log($"Initialized {copy.GetType()} upgrade");
    }
    public void AddAbilityUpgrade(AbilityUpgrade ability)
    {
        AbilityUpgrade component = (AbilityUpgrade) CopyUpgrade(ability, gameObject);
        component.OrderNumber = abilityCount;
        abilityCount++;
        component.Initialize();
    }
    private Upgrade CopyUpgrade(Upgrade original, GameObject destination)
    {
        // Get and assign values from the prefab to the new component through reflection
        System.Type type = original.GetType();
        Upgrade copy = destination.AddComponent(type) as Upgrade;

        System.Reflection.FieldInfo[] fields = type.GetFields();
        foreach(System.Reflection.FieldInfo field in fields)
        {
            field.SetValue(copy, field.GetValue(original));
        }
        return copy;
    }
}
