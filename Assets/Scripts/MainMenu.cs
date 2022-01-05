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
            Debug.Log("Attempting connection.");
            manager.networkAddress = ipField.text;
            manager.StartClient();
            PlayerPrefs.SetString("PlayerName", nameField.text);
            joinButton.interactable = false;
        }
    }
    public void StartHost()
    {
        manager.StartHost();
        PlayerPrefs.SetString("PlayerName", nameField.text);
        hostButton.interactable = false;
    }
}
