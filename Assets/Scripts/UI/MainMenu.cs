using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;
using UnityEngine.UI;
using Steamworks;
using Telepathy;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button joinButton;
    [SerializeField] private Button hostButton;
    [SerializeField] private TMP_InputField ipField;
    [SerializeField] private TMP_InputField nameField;

    private NetworkManager manager;
    private bool hasAttemptedJoin;
    private TextMeshProUGUI joinText;

    private const string HostAddressKey = "HostAddress";

    protected Callback<LobbyCreated_t> lobbyCreated;
    protected Callback<GameLobbyJoinRequested_t> joinRequested;
    protected Callback<LobbyEnter_t> lobbyEntered;

    private void Start()
    {
        manager = NetworkManager.singleton;
        joinText = joinButton.GetComponentInChildren<TextMeshProUGUI>();
        
        //if (SteamManager.Initialized)
        //{
        //    lobbyCreated = Callback<LobbyCreated_t>.Create(OnSteamLobbyCreated);
        //    joinRequested = Callback<GameLobbyJoinRequested_t>.Create(OnSteamJoinRequested);
        //    lobbyEntered = Callback<LobbyEnter_t>.Create(OnSteamLobbyEntered);
        //}
    }
    private void OnServerInitialized()
    {
        manager.StartServer();
        Debug.Log("Server starting");
    }
    public void JoinServer()
    {
        //if (SteamManager.Initialized) {
        //    Debug.Log("Steam manager is initialized.");
        //    return; 
        //}
        if (string.IsNullOrWhiteSpace(nameField.text))
        {
            Debug.Log("Name field empty.");
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
        //if (GetComponent<SteamManager>().isActiveAndEnabled)
        //{
        //    SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypePublic, manager.maxConnections);
        //    string name;
        //    if (string.IsNullOrWhiteSpace(nameField.text))
        //    {
        //        name = SteamFriends.GetPersonaName();
        //    }
        //    else
        //    {
        //        name = nameField.text;
        //    }
        //    PlayerPrefs.SetString("PlayerName", name);
        //    hostButton.interactable = false;
        //}
        if(!string.IsNullOrWhiteSpace(nameField.text))
        {
            Debug.Log("Starting host");
            manager.StartHost();
            PlayerPrefs.SetString("PlayerName", nameField.text);
            hostButton.interactable = false;
        }
    }
    //public void OnSteamLobbyCreated(LobbyCreated_t callback)
    //{
    //    if(callback.m_eResult != EResult.k_EResultOK)
    //    {
    //        hostButton.interactable = true;
    //        return;
    //    }
    //    manager.StartHost();

    //    SteamMatchmaking.SetLobbyData(new CSteamID(callback.m_ulSteamIDLobby), HostAddressKey, SteamUser.GetSteamID().ToString());
    //}
    //public void OnSteamJoinRequested(GameLobbyJoinRequested_t callback)
    //{
    //    SteamMatchmaking.JoinLobby(callback.m_steamIDLobby);
    //}
    //public void OnSteamLobbyEntered(LobbyEnter_t callback)
    //{
    //    if (NetworkServer.active) { return; }

    //    string hostAddress = SteamMatchmaking.GetLobbyData(new CSteamID(callback.m_ulSteamIDLobby), HostAddressKey);
    //    manager.networkAddress = hostAddress;
    //    manager.StartClient();
    //    string name;
    //    if (string.IsNullOrWhiteSpace(nameField.text))
    //    {
    //        name = SteamFriends.GetPersonaName();
    //    }
    //    else
    //    {
    //        name = nameField.text;
    //    }
    //    PlayerPrefs.SetString("PlayerName", name);
    //}
}
