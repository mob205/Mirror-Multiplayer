using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class UpgradeManager : NetworkBehaviour
{
    [SerializeField] List<int> upgradePaths = new List<int>() { 3, 2 };

    private static readonly Dictionary<string, GameObject> upgradesByID = new Dictionary<string, GameObject>();
    private readonly Dictionary<string, UpgradeSlot> slotsByID = new Dictionary<string, UpgradeSlot>();

    Dictionary<NetworkConnection, List<string>> serverPlayerUpgrades = new Dictionary<NetworkConnection, List<string>>();
    //string[][] currentUpgrades;
    public void Initialize()
    {
        Debug.Log("Initializing");
        InitializeDictionaries();
        GetAvailableUpgrades();
    }

    private void InitializeDictionaries()
    {
        if (upgradesByID.Count == 0)
        {
            foreach (var child in transform.GetComponentsInChildren<UpgradeSlot>())
            {
                upgradesByID.Add(child.upgradeID, child.upgradePrefab);
                slotsByID.Add(child.upgradeID, child);
            }
        }
    }
    //private void InitializeArray()
    //{
    //    currentUpgrades = new string[upgradePaths.Count][];
    //    for (int i = 0; i < upgradePaths.Count; i++)
    //    {
    //        currentUpgrades[i] = new string[upgradePaths[i]];
    //    }
    //}
    [Command(requiresAuthority = false)]
    private void GetAvailableUpgrades(NetworkConnectionToClient conn = null)
    {
        var availableUpgrades = new List<string>();
        if (serverPlayerUpgrades.ContainsKey(conn))
        {
            Debug.Log("unimplemented");
        }
        else
        {
            serverPlayerUpgrades.Add(conn, new List<string>());
            foreach (Transform child in transform)
            {
                availableUpgrades.Add(child.GetComponent<UpgradeSlot>().upgradeID);
            }
        }
        TargetDisplayUpgrades(conn, availableUpgrades.ToArray());
    }
    [TargetRpc]
    private void TargetDisplayUpgrades(NetworkConnection target, string[] availableUpgrades)
    {
        Debug.Log("Last client received upgrade: " + availableUpgrades[availableUpgrades.Length - 1]);
    }
    public static GameObject GetUpgradeFromId(string id)
    {
        return upgradesByID[id];
    }
}
