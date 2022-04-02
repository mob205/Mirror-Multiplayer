using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : NetworkBehaviour
{
    [SerializeField] private float maxHealth;

    [SyncVar] private float currentHealth;
    public float CurrentHealth { get { return currentHealth;  } }
    public float MaxHealth { get { return maxHealth; } set { maxHealth = value; } }
    public static event Action<Health, uint> OnDeath;
    public void Start()
    {
        currentHealth = maxHealth;
    }
    [Server]
    public void Damage(float amount, GameObject attacker)
    {
        currentHealth -= amount;
        if(currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        if(currentHealth <= 0)
        {
            var killerID = attacker.GetComponent<NetworkIdentity>().netId;
            RpcStartDeath(killerID);
        }
    }
    [ClientRpc]
    public void RpcStartDeath(uint killerID)
    {
        gameObject.SetActive(false);
        OnDeath?.Invoke(this, killerID);
    }
}
