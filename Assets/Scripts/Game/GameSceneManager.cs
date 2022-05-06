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

    public static event Action<uint> OnWinRound;
    public static string lastWinnerName;

    public int WinReward
    {
        get
        {
            return playerWinReward;
        }
    }

    private CustomNetworkManager nm;
    private bool hasWonRound;
    private bool hasWonGame;
    private void Start()
    {
        instance = this;
        if (isServer)
        {
            nm = CustomNetworkManager.singleton;
            PlayerHealth.OnPlayerDeath += OnPlayerDeath;
            CustomNetworkManager.OnPlayerDisconnect += OnPlayerDisconnect;
            PointTracker.OnGameWin += OnGameWin;
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
            lastWinnerName = alivePlayers[0].GetComponent<PlayerDisplayer>().playerName;
            StartCoroutine(StartPlayerWinRound(winnerID));
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
            StartCoroutine(StartPlayerWinRound(winnerID));
        }
    }
    [Server]
    private IEnumerator StartPlayerWinRound(uint winnerID)
    {
        if (hasWonRound) { yield break; }
        if (isServerOnly) { OnWinRound?.Invoke(winnerID); }
        hasWonRound = !hasWonRound;
        RpcAlertWinRound(winnerID, lastWinnerName);
        yield return new WaitForSeconds(sceneChangeDelay);
        ChangeScene();
    }
    private void ChangeScene()
    {
        if (hasWonGame)
        {
            nm.ServerChangeScene(nm.winScene);
        }
        else
        {
            nm.ServerChangeScene(nm.upgradeScene);
        }
    }
    private void OnGameWin(NetworkConnection conn)
    {
        hasWonGame = true;
    }

    [ClientRpc]
    public void RpcAlertWinRound(uint playerID, string playerName)
    {
        lastWinnerName = playerName;
        OnWinRound?.Invoke(playerID);
    }
    private void OnDestroy()
    {
        if (isServer)
        {
            PlayerHealth.OnPlayerDeath -= OnPlayerDeath;
            CustomNetworkManager.OnPlayerDisconnect -= OnPlayerDisconnect;
            PointTracker.OnGameWin -= OnGameWin;
        }
    }
}
