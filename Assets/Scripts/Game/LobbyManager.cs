using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyManager : NetworkBehaviour
{
    private CustomNetworkManager networkManager;

    Dictionary<NetworkConnectionToClient, bool> readyStates = new Dictionary<NetworkConnectionToClient, bool>();


    public override void OnStartServer()
    {
        networkManager = CustomNetworkManager.singleton;
        CustomNetworkManager.OnPlayerConnect += OnPlayerJoin;
        CustomNetworkManager.OnPlayerDisconnect += OnPlayerLeave;
    }

    private void OnDisable()
    {
        if(NetworkServer.active)
        {
            CustomNetworkManager.OnPlayerConnect -= OnPlayerJoin;
            CustomNetworkManager.OnPlayerDisconnect -= OnPlayerLeave; 
        }
    }

    [Server]
    private void OnPlayerJoin(NetworkIdentity identity)
    {
        readyStates.Add(identity.connectionToClient, false);
    }
    [Server]
    private void OnPlayerLeave(NetworkIdentity identity)
    {
        readyStates.Add(identity.connectionToClient, false);
    }

    [Client]
    public void ClientToggleReadyPlayer()
    {
        ToggleReadyPlayer();
    }

    [Command(requiresAuthority = false)]
    public void ToggleReadyPlayer(NetworkConnectionToClient conn = null)
    {
        if(readyStates.ContainsKey(conn))
        {
            readyStates[conn] = !readyStates[conn];
        }
        else
        {
            readyStates.Add(conn, true);
        }

        foreach(var playerState in readyStates)
        {
            if(playerState.Value == false)
            {
                return;
            }
        }
        networkManager.StartLevel();
    }
}
