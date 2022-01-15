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
    [SerializeField] GameObject upgradePlayerPrefab;

    public GameObject[] weapons;
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
        if(SceneManager.GetActiveScene().path != upgradeScene)
        {
            base.OnServerAddPlayer(conn);
            conn.identity.GetComponent<PlayerCombat>().CmdSetWeapon(Random.Range(0, weapons.Length));
        }
        else
        {
            GameObject player = Instantiate(upgradePlayerPrefab);
            NetworkServer.AddPlayerForConnection(conn, player);
        }
    }
    public override void OnClientSceneChanged(NetworkConnection conn)
    {
        base.OnClientSceneChanged(conn);
    }
}
