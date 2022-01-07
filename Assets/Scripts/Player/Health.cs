using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : NetworkBehaviour
{
    [SerializeField] private float maxHealth;

    [SyncVar] private float currentHealth;
    public float CurrentHealth { get { return currentHealth;  } }
    public float MaxHealth { get { return maxHealth; } }
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
            NetworkServer.Destroy(gameObject);
        }
    }
}
