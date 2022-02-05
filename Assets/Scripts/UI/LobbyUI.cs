using Mirror;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUI : NetworkBehaviour
{
    [SerializeField] private TextMeshProUGUI playerCountText;
    [SerializeField] private GameObject startButton;

    private CustomNetworkManager nm;
    [SyncVar] int numPlayers;
    private void Start()
    {
        nm = CustomNetworkManager.singleton;
        if (isServer)
        {
            startButton.SetActive(true);
        }
    }
    private void Update()
    {
        if(isServer && numPlayers != nm.numPlayers)
        {
            numPlayers = nm.numPlayers;
        }
        playerCountText.text = $"{numPlayers} / {nm.maxConnections}";
    }
    public void StartGame()
    {
        nm.StartLevel();
    }
}
