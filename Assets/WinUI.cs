using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Mirror;

public class WinUI : NetworkBehaviour
{
    [SerializeField] TextMeshProUGUI text;

    private void Start()
    {
        if (isServer)
        {
            GameSceneManager.OnPlayerWin += CmdGetPlayerName;
        }
    }
    [ClientRpc]
    private void DisplayWinMessage(string playerName)
    {
        text.gameObject.SetActive(true);
        text.text = $"{playerName} WINS!";
    }
    [Command(requiresAuthority = false)]
    private void CmdGetPlayerName(uint playerID)
    {
        var player = NetworkServer.spawned[playerID].GetComponent<PlayerDisplayer>();
        DisplayWinMessage(player.playerName);
    }
    private void OnDestroy()
    {
        if (isServer)
        {
            GameSceneManager.OnPlayerWin -= CmdGetPlayerName;
        }
    }
}
