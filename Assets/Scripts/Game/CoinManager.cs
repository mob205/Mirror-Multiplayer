using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinManager : NetworkBehaviour
{
    public static CoinManager instance;
    private static Dictionary<NetworkConnection, int> coins = new Dictionary<NetworkConnection, int>();
    public static int ClientCoins { get; set; } = 0;
    public void Start()
    {
        if (instance)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    [Server]
    public void ModifyCoins(NetworkConnection conn, int amount)
    {
        if (coins.ContainsKey(conn))
        {
            coins[conn] += amount;
        }
        else
        {
            coins.Add(conn, amount);
        }
        UpdateClientCoins(conn, coins[conn]);
    }
    [Server]
    public int GetCoins(NetworkConnection conn)
    {
        if (coins.ContainsKey(conn))
        {
            return coins[conn];
        }
        else
        {
            return 0;
        }
    }
    [TargetRpc]
    private void UpdateClientCoins(NetworkConnection conn, int amount)
    {
        ClientCoins = amount;
    }
    [Server]
    public static void ResetStatics()
    {
        coins.Clear();
    }
}
