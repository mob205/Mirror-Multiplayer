using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameSceneManager : NetworkBehaviour
{
    public static GameSceneManager instance;

    [SerializeField] private float sceneChangeDelay = 5;
    [SerializeField] private int playerKillReward = 100;
    [SerializeField] private int playerWinReward = 100;

    public int WinReward
    {
        get
        {
            return playerWinReward;
        }
    }

    private CustomNetworkManager nm;
    public static event Action<uint> OnPlayerWin;
    bool hasWon;
    private void Start()
    {
        instance = this;
        if (isServer)
        {
            nm = CustomNetworkManager.singleton;
            PlayerHealth.OnPlayerDeath += OnPlayerDeath;
            CustomNetworkManager.OnPlayerDisconnect += OnPlayerDisconnect;
        }
    }
    [Server]
    private void OnPlayerDeath(PlayerHealth player, uint killer)
    {
        var cm = CoinManager.instance;
        NetworkConnectionToClient killerConn = null;
        if (NetworkServer.spawned.ContainsKey(killer))
        {
            killerConn = NetworkServer.spawned[killer].connectionToClient;
            cm.ModifyCoins(killerConn, playerKillReward);
        }

        var alivePlayers = FindObjectsOfType<PlayerHealth>();
        if (alivePlayers.Length == 1)
        {
            var winnerID = alivePlayers[0].netId;
            StartCoroutine(StartPlayerWin(winnerID));
            if(killerConn != null)
            {
                cm.ModifyCoins(killerConn, playerWinReward);
            }
        }
    }
    [Server]
    private void OnPlayerDisconnect(NetworkIdentity player)
    {
        var alivePlayers = FindObjectsOfType<PlayerHealth>().ToList();
        alivePlayers.Remove(player.GetComponent<PlayerHealth>());
        if(alivePlayers.Count == 1)
        {
            var winnerID = alivePlayers[0].netId;
            StartCoroutine(StartPlayerWin(winnerID));
        }
    }
    [Server]
    private IEnumerator StartPlayerWin(uint winnerID)
    {
        if (hasWon) { yield break; }
        if (isServerOnly) { OnPlayerWin?.Invoke(winnerID); }
        hasWon = !hasWon;
        RpcAlertWin(winnerID);
        yield return new WaitForSeconds(sceneChangeDelay);
        nm.ServerChangeScene(nm.upgradeScene);
    }
    [ClientRpc]
    public void RpcAlertWin(uint playerID)
    {
        OnPlayerWin?.Invoke(playerID);
    }
    private void OnDestroy()
    {
        if (isServer)
        {
            PlayerHealth.OnPlayerDeath -= OnPlayerDeath;
            CustomNetworkManager.OnPlayerDisconnect -= OnPlayerDisconnect;
        }
    }
}
