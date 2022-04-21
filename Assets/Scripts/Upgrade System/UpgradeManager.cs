using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class UpgradeManager : NetworkBehaviour
{
    [SerializeField] List<int> upgradePaths = new List<int>() { 3, 2 };

    private static readonly Dictionary<string, Upgrade> upgradesByID = new Dictionary<string, Upgrade>();
    private readonly Dictionary<string, UpgradeSlot> slotsByID = new Dictionary<string, UpgradeSlot>();

    private static Dictionary<NetworkConnection, List<string>> serverPlayerUpgrades = new Dictionary<NetworkConnection, List<string>>();
    private static Dictionary<NetworkConnection, List<string>> serverPlayerAvailableUpgrades = new Dictionary<NetworkConnection, List<string>>();

    private UpgradeUI ui;
    private CoinManager cm;
    public void Initialize()
    {
        ui = FindObjectOfType<UpgradeUI>();
        cm = CoinManager.instance;
        InitializeDictionaries();
        CmdGetAvailableUpgrades();
    }
    private void InitializeDictionaries()
    {
        var hasInitialized = upgradesByID.Count > 0;
        // Walk through the UpgradeManager's children hierarchy to get the dictionaries
        foreach (var child in transform.GetComponentsInChildren<UpgradeSlot>())
        {
            if (!hasInitialized && child.upgradePrefab)
            {
                upgradesByID.Add(child.upgradeID, child.upgradePrefab.GetComponent<Upgrade>());
            }
            slotsByID.Add(child.upgradeID, child);
        }
    }
    [Command(requiresAuthority = false)]
    private void CmdGetAvailableUpgrades(NetworkConnectionToClient conn = null)
    {
        var availableUpgrades = new List<string>();
        if (!serverPlayerUpgrades.ContainsKey(conn))
        {
            // First time player requests upgrades
            serverPlayerUpgrades.Add(conn, new List<string>());
            serverPlayerAvailableUpgrades.Add(conn, new List<string>());
        }
        string classUpgrade = "";
        if (serverPlayerUpgrades[conn].Count > 0)
        {
            var playerPaths = SortIntoPaths(serverPlayerUpgrades[conn]);
            availableUpgrades.AddRange(GetNextUpgrades(playerPaths));

            // Class upgrade should always be the player's first upgrade.
            classUpgrade = serverPlayerUpgrades[conn][0];
            if (playerPaths.Count < upgradePaths.Count)
            {
                availableUpgrades.AddRange(GetBasicUpgrades(playerPaths, classUpgrade));
            }

            // Sort the available upgrades so each path stays in the same place when buying upgrades
            availableUpgrades = OrderByHierarchy(availableUpgrades, classUpgrade);
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
        if (!string.IsNullOrEmpty(classUpgrade))
        {
            TargetUpdatePreview(conn, classUpgrade);
        }
    }
    private List<string> GetNextUpgrades(List<List<string>> playerPaths)
    {
        var output = new List<string>();
        var currentMaxPath = 0;
        for (int i = 0; i < playerPaths.Count; i++)
        {
            // Get the next upgrade in each path, if the path is not at max as defined by upgradePaths.
            var path = playerPaths[i];
            if (path.Count < upgradePaths[currentMaxPath])
            {
                var lastPathUpgrade = slotsByID[path[path.Count - 1]];
                output.Add(lastPathUpgrade.nextUpgradeID);
            }
            else
            {
                // If playerPath's biggest path is not equal to the greatest allowed path by
                // upgradePaths, then it should not use the next path size in the next iteration. 
                currentMaxPath++;
            }
        }
        return output;
    }
    // Gets the level 1 upgrades still available if not all paths are selected.
    private List<string> GetBasicUpgrades(List<List<string>> playerPaths, string classUpgrade)
    {
        var output = new List<string>();
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
                output.Add(basicUpgrade);
            }
        }
        return output;
    }
    // Sorts the upgrades by the order they appear in the hierarchy so they can be displayed in a logical order.
    private List<string> OrderByHierarchy(List<string> availableUpgrades, string classUpgrade)
    {
        var output = new List<string>();
        foreach (var child in slotsByID[classUpgrade].GetComponentsInChildren<UpgradeSlot>())
        {
            if (availableUpgrades.Contains(child.upgradeID))
            {
                output.Add(child.upgradeID);
            }
        }
        return output;
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
        var upgradeCost = slotsByID[upgradeID].cost;
        if (serverPlayerAvailableUpgrades[conn].Contains(upgradeID) && cm.GetCoins(conn) >= upgradeCost)
        {
            serverPlayerUpgrades[conn].Add(upgradeID);
            TargetUpdateAvailableUpgrades(conn);
            cm.ModifyCoins(conn, -upgradeCost);
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
    [TargetRpc]
    private void TargetUpdatePreview(NetworkConnection target, string classUpgradeID)
    {
        ui.UpdatePreview(classUpgradeID);
    }
    public static Upgrade GetUpgradeFromID(string id)
    {
        return upgradesByID[id];
    }
    public UpgradeSlot GetSlotFromID(string id)
    {
        return slotsByID[id];
    }
    public static string[] GetUpgradesForConn(NetworkConnection conn)
    {
        if (serverPlayerUpgrades.ContainsKey(conn))
        {
            return serverPlayerUpgrades[conn].ToArray();
        }
        return new string[0];
    }
    [Server]
    public static void ResetStatics()
    {
        serverPlayerAvailableUpgrades.Clear();
        serverPlayerUpgrades.Clear();
    }
}
