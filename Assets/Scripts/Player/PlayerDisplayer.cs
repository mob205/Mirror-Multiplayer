using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDisplayer : NetworkBehaviour
{
    [SyncVar(hook = "OnReceiveName")] public string playerName;
    public static event Action<GameObject> OnStartPlayerUI;
    public static event Action<GameObject> OnRemovePlayerUI;

    private void Start()
    {
        if (isLocalPlayer && string.IsNullOrEmpty(playerName))
        {
            // Send the name to the server and then display to all clients if local player's name is not set
            CmdSendName(PlayerPrefs.GetString("PlayerName"));
        }
        else if(!isLocalPlayer && !string.IsNullOrEmpty(playerName))
        {
            OnStartPlayerUI?.Invoke(gameObject);
        }
    }
    [Command]
    public void CmdSendName(string name)
    {
        playerName = name;
    }
    public void OnReceiveName(string oldName, string newName)
    {
        OnStartPlayerUI?.Invoke(gameObject);
    }
    public override void OnStopClient()
    {
        OnRemovePlayerUI?.Invoke(gameObject);
    }
}
