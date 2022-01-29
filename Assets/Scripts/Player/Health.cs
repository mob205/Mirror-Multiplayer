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
    public float MaxHealth { get { return maxHealth; } }
    public static event Action<Health> OnDeath;
    public void Start()
    {
        currentHealth = maxHealth;
    }
    [Server]
    public void Damage(float amount)
    {
        currentHealth -= amount;
        if(currentHealth <= 0)
        {
            //gameObject.SetActive(false);
            RpcStartDeath();
        }
    }
    [ClientRpc]
    public void RpcStartDeath()
    {
        gameObject.SetActive(false);
        OnDeath?.Invoke(this);
    }
    public override void OnStopClient()
    {
        OnDeath?.Invoke(this);
    }
}
