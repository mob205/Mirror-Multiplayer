using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinItem : Item
{
    [SerializeField] private int minAmount;
    [SerializeField] private int maxAmount;

    [Client]
    protected override void LocalActivate(Collider2D collision)
    {
        Debug.Log("Coin collected.");
    }
    [Server]
    protected override void ServerActivate(Collider2D collision)
    {
        var conn = collision.GetComponent<NetworkIdentity>().connectionToClient;
        var amount = Random.Range(minAmount, maxAmount + 1);
        CoinManager.instance.ModifyCoins(conn, amount);
    }
}
