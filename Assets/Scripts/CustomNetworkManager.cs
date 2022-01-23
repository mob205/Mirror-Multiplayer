using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;
using System;
using Random = UnityEngine.Random;

public class CustomNetworkManager : NetworkManager
{
    [Scene] [SerializeField] string upgradeScene;
    [Scene] [SerializeField] string lobbyScene;
    [SerializeField] GameObject upgradePlayerPrefab;
    [SerializeField] GameObject lobbyPlayerPrefab;

    new public static CustomNetworkManager singleton;

    //public static event Action OnPlayerAdded;

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
        if (Input.GetKeyDown(KeyCode.X))
        {
            Debug.Log("Switching to main scene. FOR DEBUG ONLY");
            ServerChangeScene("Scene");
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
}

