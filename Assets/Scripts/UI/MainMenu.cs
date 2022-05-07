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
    private bool hasAttemptedJoin;
    private TextMeshProUGUI joinText;

    private void Start()
    {
        manager = NetworkManager.singleton;
        joinText = joinButton.GetComponentInChildren<TextMeshProUGUI>();
    }
    public void JoinServer()
    {
        if (string.IsNullOrWhiteSpace(nameField.text))
        {
            return;
        }
        if (hasAttemptedJoin)
        {
            manager.StopClient();
            hasAttemptedJoin = false;
            joinText.text = "JOIN";
            return;
        }
        if (!string.IsNullOrEmpty(ipField.text))
        {
            PlayerPrefs.SetString("PlayerName", nameField.text);
            manager.networkAddress = ipField.text;
            joinText.text = "CANCEL";
            hasAttemptedJoin = true;
            manager.StartClient();
        }
    }
    public void StartHost()
    {
        if (string.IsNullOrWhiteSpace(nameField.text))
        {
            return;
        }
        PlayerPrefs.SetString("PlayerName", nameField.text);
        manager.StartHost();
        hostButton.interactable = false;
    }
}
