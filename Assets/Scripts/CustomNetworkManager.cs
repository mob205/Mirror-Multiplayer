using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class CustomNetworkManager : NetworkManager
{
    public GameObject[] weapons;
    new public static CustomNetworkManager singleton;

    public override void Awake()
    {
        base.Awake();
        singleton = NetworkManager.singleton as CustomNetworkManager;
    }
    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        base.OnServerAddPlayer(conn);
        conn.identity.GetComponent<PlayerCombat>().CmdSetWeapon(Random.Range(0, weapons.Length));
    }
}

