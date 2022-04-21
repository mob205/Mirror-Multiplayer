using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;
using System;

public class CustomNetworkManager : NetworkManager
{
    [Scene] [SerializeField] public string upgradeScene;
    [Scene] [SerializeField] public string lobbyScene;
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
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Debug.Log("Switching to upgrade scene. FOR DEBUG ONLY");
            ServerChangeScene(upgradeScene);
        }
        else if (Input.GetKeyDown(KeyCode.X))
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
        ServerChangeScene("Scene");
    }
}