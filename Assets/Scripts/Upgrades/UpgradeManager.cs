using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class UpgradeManager : NetworkBehaviour
{
    [SerializeField] List<int> upgradePaths = new List<int>() { 3, 2 };
    //[SerializeField] List<UpgradeSlot> testUpgrades;

    private static readonly Dictionary<string, GameObject> upgradesByID = new Dictionary<string, GameObject>();
    private static readonly Dictionary<string, UpgradeSlot> slotsByID = new Dictionary<string, UpgradeSlot>();

    Dictionary<NetworkConnection, List<string>> serverPlayerUpgrades = new Dictionary<NetworkConnection, List<string>>();
    Dictionary<NetworkConnection, List<string>> serverPlayerAvailableUpgrades = new Dictionary<NetworkConnection, List<string>>();

    private UpgradeUI ui;
    public void Initialize()
    {
        ui = FindObjectOfType<UpgradeUI>();
        InitializeDictionaries();
        CmdGetAvailableUpgrades();
        //DebugRequestUpgrades();
    }
    //private void DebugRequestUpgrades()
    //{
    //    var testUpgradeIDs = new List<string>();
    //    foreach(var upgrade in testUpgrades)
    //    {
    //        testUpgradeIDs.Add(upgrade.upgradeID);
    //    }
    //    CmdGetAvailableUpgrades(testUpgradeIDs.ToArray());
    //}
    private void InitializeDictionaries()
    {
        // Walk through the UpgradeManager's children hierarchy to get the dictionaries
        if (upgradesByID.Count == 0)
        {
            foreach (var child in transform.GetComponentsInChildren<UpgradeSlot>())
            {
                upgradesByID.Add(child.upgradeID, child.upgradePrefab);
                slotsByID.Add(child.upgradeID, child);
            }
        }
    }
    [Command(requiresAuthority = false)]
    private void CmdGetAvailableUpgrades(/*string[] testUpgrades,*/ NetworkConnectionToClient conn = null)
    {
        //serverPlayerUpgrades[conn] = testUpgrades.ToList();
        var availableUpgrades = new List<string>();
        if (!serverPlayerUpgrades.ContainsKey(conn))
        {
            // First time player requests upgrades
            serverPlayerUpgrades.Add(conn, new List<string>());
            serverPlayerAvailableUpgrades.Add(conn, new List<string>());
        }
        if (serverPlayerUpgrades[conn].Count > 0)
        {
            var playerPaths = SortIntoPaths(serverPlayerUpgrades[conn]);
            var currentMaxPath = 0;
            for (int i = 0; i < playerPaths.Count; i++)
            {
                // Get the next upgrade in each path, if the path is not at max as defined by upgradePaths.
                var path = playerPaths[i];
                if (path.Count < upgradePaths[currentMaxPath])
                {
                    var lastPathUpgrade = slotsByID[path[path.Count - 1]];
                    availableUpgrades.Add(lastPathUpgrade.nextUpgradeID);
                }
                else
                {
                    // If playerPath's biggest path is not bigger than the greatest allowed path by
                    // upgradePaths, then it should not use the next path in the next iteration. 
                    currentMaxPath++;
                }
            }
            // Make other level 1 upgrades available if all paths are not yet selected
            if (playerPaths.Count < upgradePaths.Count)
            {
                // Class upgrade should always be the player's first upgrade.
                var classUpgrade = serverPlayerUpgrades[conn][0];
                foreach (Transform basicUpgradeSlot in slotsByID[classUpgrade].transform)
                {
                    // Only make the upgrade available if it's not already one of the player's upgrades.
                    var isSelected = false;
                    var basicUpgrade = basicUpgradeSlot.GetComponent<UpgradeSlot>().upgradeID;
                    foreach (var path in playerPaths)
                    {
                        if (path.Contains(basicUpgrade))
                        {
                            isSelected = true;
                        }
                    }
                    if (!isSelected)
                    {
                        availableUpgrades.Add(basicUpgrade);
                    }
                }
            }
        }
        else
        {
            // Return default paths
            foreach (Transform child in transform)
            {
                availableUpgrades.Add(child.GetComponent<UpgradeSlot>().upgradeID);
            }
        }
        serverPlayerAvailableUpgrades[conn] = availableUpgrades;
        TargetDisplayUpgrades(conn, availableUpgrades.ToArray());
    }
    private List<List<string>> SortIntoPaths(List<string> upgrades)
    {
        var output = new List<List<string>>();
        foreach (var upgrade in upgrades)
        {
            var slot = slotsByID[upgrade];
            // Upgrade list is in order of purchase, so level 1's should always come first.
            // Level 1 upgrades are a new path. Level 0 class upgrades should be ignored.
            if (slot.level == 1)
            {
                output.Add(new List<string>() { upgrade });
            }
            // Level 2 and 3 upgrades should be added to the path with its parent upgrade.
            else if (slot.level >= 1)
            {
                foreach (var path in output)
                {
                    if (path.Contains(slot.prereqUpgradeID))
                    {
                        path.Add(slot.upgradeID);
                    }
                }
            }
        }
        // Sort by length of path. Level 3 path before Level 2, etc.
        output = output.OrderByDescending(l => l.Count()).ToList();
        if (output.Count > upgradePaths.Count)
        {
            Debug.LogError("Player has more paths than allowed!");
        }
        return output;
    }
    [Command(requiresAuthority = false)]
    public void CmdRequestAddUpgrade(string upgradeID, NetworkConnectionToClient conn = null)
    {
        if (serverPlayerAvailableUpgrades[conn].Contains(upgradeID))
        {
            serverPlayerUpgrades[conn].Add(upgradeID);
            TargetUpdateAvailableUpgrades(conn);
        }
        else
        {
            Debug.Log(conn + " requested an unavailable upgrade.");
        }
    }
    [TargetRpc]
    private void TargetUpdateAvailableUpgrades(NetworkConnection target)
    {
        // Can't go straight from CmdRequestAddUpgrade to CmdGetAvailableUpgrades because the connection to client is lost.
        CmdGetAvailableUpgrades();
    }
    [TargetRpc]
    private void TargetDisplayUpgrades(NetworkConnection target, string[] availableUpgrades)
    {
        ui.DisplayUpgrades(availableUpgrades);
    }
    public static GameObject GetUpgradeFromID(string id)
    {
        return upgradesByID[id];
    }
    public UpgradeSlot GetSlotFromID(string id)
    {
        return slotsByID[id];
    }
}
