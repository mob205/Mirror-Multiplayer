using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class UpgradeManager : NetworkBehaviour
{
    [SerializeField] List<int> upgradePaths = new List<int>() { 3, 2 };

    private static readonly Dictionary<string, UpgradeSlot> upgradeIDs = new Dictionary<string, UpgradeSlot>();

    Dictionary<NetworkConnection, string[]> serverPlayerUpgrades = new Dictionary<NetworkConnection, string[]>();
    //string[][] currentUpgrades;
    // Start is called before the first frame update
    public override void OnStartClient()
    {
        Debug.Log("t");
        InitializeUpgradeDictionary();
        //InitializeArray();
        GetAvailableUpgrades();
    }
    private void InitializeUpgradeDictionary()
    {
        if (upgradeIDs.Count == 0)
        {
            foreach (var child in transform.GetComponentsInChildren<UpgradeSlot>())
            {
                upgradeIDs.Add(child.upgradeID, child);
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
    private void GetAvailableUpgrades()
    {
        var availableUpgrades = new List<string>();
        if (serverPlayerUpgrades.ContainsKey(connectionToClient))
        {
            Debug.Log("unimplemented");
        }
        else
        {
            serverPlayerUpgrades.Add(connectionToClient, null);
            foreach (Transform child in transform)
            {
                availableUpgrades.Add(child.GetComponent<UpgradeSlot>().upgradeID);
            }
        }
        DisplayAvailableUpgrades(connectionToClient, availableUpgrades.ToArray());
    }
    [TargetRpc]
    private void DisplayAvailableUpgrades(NetworkConnection target, string[] availableUpgrades)
    {
        Debug.Log(availableUpgrades);
    }
    public static GameObject GetUpgradeFromId(string id)
    {
        return upgradeIDs[id].upgradePrefab;
    }
}
