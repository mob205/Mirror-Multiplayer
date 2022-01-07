using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDisplayer : NetworkBehaviour
{
    [SyncVar(hook = "DisplayName")] public string playerName;

    private HealthUI ui;
    void Start()
    {
        ui = FindObjectOfType<HealthUI>();
        if (isLocalPlayer && string.IsNullOrEmpty(playerName))
        {
            // Send the name to the server and then display to all clients if it is the local player's first time running Start()
            CmdSendName(PlayerPrefs.GetString("PlayerName"));
        } 
        else if(!isLocalPlayer && !string.IsNullOrEmpty(playerName))
        {
            // Display the name already set to the server if client joining later
            DisplayName(playerName, playerName);
        }
    }
    [Command]
    public void CmdSendName(string name)
    {
        playerName = name;
    }
    public void DisplayName(string oldName, string newName)
    {
        if(ui != null)
        {
            ui.AddPlayerUI(gameObject, newName);
        }
    }
    public override void OnStopClient()
    {
        ui.RemovePlayerUI(gameObject);
    }
}
