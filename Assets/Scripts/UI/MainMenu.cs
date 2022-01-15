using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button joinButton;
    [SerializeField] private Button hostButton;
    [SerializeField] private TMP_InputField ipField;
    [SerializeField] private TMP_InputField nameField;

    private NetworkManager manager;

    private void Start()
    {
        manager = NetworkManager.singleton;

    }
    public void JoinServer()
    {
        if (!string.IsNullOrEmpty(ipField.text))
        {
            PlayerPrefs.SetString("PlayerName", nameField.text);
            manager.networkAddress = ipField.text;
            manager.StartClient();
            joinButton.interactable = false;
        }
    }
    public void StartHost()
    {
        PlayerPrefs.SetString("PlayerName", nameField.text);
        manager.StartHost();
        hostButton.interactable = false;
    }
}