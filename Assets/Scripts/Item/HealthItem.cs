using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthItem : Item
{
    [SerializeField] private int minAmount;
    [SerializeField] private int maxAmount;

    [Client]
    protected override void RpcLocalActivate(GameObject collider)
    {
        Debug.Log("Health collected.");
    }
    [Server]
    protected override void ServerActivate(Collider2D collision)
    {
        var health = collision.GetComponent<Health>();
        var amount = Random.Range(minAmount, maxAmount + 1);
        health.Damage(-amount, gameObject);
    }
}
