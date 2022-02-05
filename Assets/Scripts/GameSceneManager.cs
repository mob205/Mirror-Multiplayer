using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameSceneManager : NetworkBehaviour
{
    [SerializeField] private float sceneChangeDelay = 5;

    private CustomNetworkManager nm;

    public static event Action<uint> OnPlayerWin;
    private void Start()
    {
        if (isServer)
        {
            nm = CustomNetworkManager.singleton;
            Health.OnDeath += OnPlayerDeath;
        }
    }
    [Server]
    private void OnPlayerDeath(Health player)
    {
        var alivePlayers = FindObjectsOfType<Health>();
        if(alivePlayers.Length == 1)
        {
            var winnerID = alivePlayers[0].netId;
            StartCoroutine(StartPlayerWin(winnerID));
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
