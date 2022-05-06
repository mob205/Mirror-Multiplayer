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
    public float DamageResistance { get; set; }

    public override void OnStartServer()
    {
        currentHealth = maxHealth;
    }
    [Server]
    public void Damage(float amount, GameObject attacker)
    {
        if(currentHealth < 0)
        {
            Debug.Log("Already dead!");
            return;
        }
        currentHealth -= amount * (1 - DamageResistance);
        if(currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        if(currentHealth <= 0)
        {
            var killerID = attacker.GetComponent<NetworkIdentity>().netId;
            StartDeath(killerID);
            RpcStartDeath(killerID);
        }
    }
    protected virtual void StartDeath(uint killerID)
    {
        gameObject.SetActive(false);
    }
    [ClientRpc]
    private void RpcStartDeath(uint killerID)
    {
        StartDeath(killerID);
    }
}
