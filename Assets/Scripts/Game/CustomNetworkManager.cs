using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;
using System;
using System.Collections.Generic;

public class CustomNetworkManager : NetworkManager
{
    [Scene] public string upgradeScene;
    [Scene] public string lobbyScene;
    [Scene] public string winScene;
    [Scene] public List<string> levelScenes;
    [SerializeField] GameObject upgradePlayerPrefab;
    [SerializeField] GameObject lobbyPlayerPrefab;

    new public static CustomNetworkManager singleton;

    public static event Action<NetworkIdentity> OnPlayerDisconnect;

    public override void Awake()
    {
        base.Awake();
        singleton = NetworkManager.singleton as CustomNetworkManager;
    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z) && Debug.isDebugBuild)
        {
            ServerChangeScene(upgradeScene);
        }
        else if (Input.GetKeyDown(KeyCode.X) && Debug.isDebugBuild)
        {
            StartLevel();
        }
    }
    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        if(SceneManager.GetActiveScene().path == upgradeScene)
        {
            GameObject player = Instantiate(upgradePlayerPrefab);
            NetworkServer.AddPlayerForConnection(conn, player);
        } 
        else if(SceneManager.GetActiveScene().path == lobbyScene)
        {
            GameObject player = Instantiate(lobbyPlayerPrefab);
            NetworkServer.AddPlayerForConnection(conn, player);
        }
        else if(SceneManager.GetActiveScene().path == winScene)
        {
            return;
        }
        else
        {
            base.OnServerAddPlayer(conn);
        }
    }
    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);
        if (SceneManager.GetActiveScene().path != lobbyScene && !Debug.isDebugBuild)
        {
            conn.Disconnect();
            SceneManager.LoadScene(0);
        }
    }
    public override void OnServerDisconnect(NetworkConnection conn)
    {
        OnPlayerDisconnect?.Invoke(conn.identity);
        base.OnServerDisconnect(conn);
    }
    [Server]
    public void StartLevel()
    {
        var randomScene = levelScenes[UnityEngine.Random.Range(0, levelScenes.Count)];
        ServerChangeScene(randomScene);
    }
}