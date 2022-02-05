using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameSceneManager : NetworkBehaviour
{
    [SerializeField] private float sceneChangeDelay = 5;
    [SerializeField] private int playerKillReward = 100;
    [SerializeField] private int playerWinReward = 100;

    private CustomNetworkManager nm;
    private CoinManager cm;

    public static event Action<uint> OnPlayerWin;
    private void Start()
    {
        if (isServer)
        {
            nm = CustomNetworkManager.singleton;
            cm = CoinManager.instance;
            Health.OnDeath += OnPlayerDeath;
        }
    }
    [Server]
    private void OnPlayerDeath(Health player, uint killer)
    {
        var killerConn = NetworkServer.spawned[killer].connectionToClient;
        cm.ModifyCoins(killerConn, playerKillReward);

        var alivePlayers = FindObjectsOfType<Health>();
        if(alivePlayers.Length == 1)
        {
            var winnerID = alivePlayers[0].netId;
            StartCoroutine(StartPlayerWin(winnerID));
            cm.ModifyCoins(killerConn, playerWinReward);
        }
    }
    [Server]
    private IEnumerator StartPlayerWin(uint winnerID)
    {
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
            Health.OnDeath -= OnPlayerDeath;
        }
    }
}
