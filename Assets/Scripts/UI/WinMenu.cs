using Mirror;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WinMenu : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI winText;
    private void Update()
    {
        winText.text = $"{GameSceneManager.lastWinnerName} wins the game!";
    }
    public void QuitGame()
    {
        NetworkManager.singleton.StopClient();
        NetworkManager.singleton.StopServer();
    }
}
