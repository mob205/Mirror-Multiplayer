using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinItem : Item
{
    [SerializeField] private int amount;

    [Client]
    protected override void LocalActivate(Collider2D collision)
    {
        Debug.Log("Coin collected.");
    }
    [Server]
    protected override void ServerActivate(Collider2D collision)
    {
        var conn = collision.GetComponent<NetworkIdentity>().connectionToClient;
        CoinManager.instance.ModifyCoins(conn, amount);
    }
}
