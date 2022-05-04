using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbilityUpgrade : Upgrade
{
    [Header("General")]
    public Sprite icon;
    public float baseCooldown;
    public string abilityID;
    [Header("Charges")]
    public int maxCharges = 1;
    public float baseCastDelay;

    public float RemainingCooldown { get; private set; } = 0;
    public float RemainingCastDelay { get; private set; } = 0;
    public float RemainingCharges { get; private set; } = 0;
    public int OrderNumber { get; set; }

    protected NetworkIdentity identity;
    private PlayerAbilities abilities;
    private PlayerMovement playerMovement;
    protected Camera cam;

    public Action<AbilityUpgrade> OnAbilityCast;

    public override void Initialize()
    {
        identity = GetComponent<NetworkIdentity>();
        abilities = GetComponent<PlayerAbilities>();
        playerMovement = GetComponent<PlayerMovement>();
        ResetCharges();
        abilities.AddAbility(this);

        if (identity.isLocalPlayer) 
        {
            cam = Camera.main; 
        }
    }
    protected virtual void Update()
    {
        UpdateCooldown();
        if (Input.GetButton($"Ability{OrderNumber}") && RemainingCastDelay <= 0 && RemainingCharges >= 1 && identity.isLocalPlayer && playerMovement.CurrentState != PlayerMovement.State.Immobilized)
        {
            var mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
            abilities.RequestAbility(abilityID, mousePos);
        }
    }
    private void UpdateCooldown()
    {
        if (RemainingCooldown > 0)
        {
            RemainingCooldown -= Time.deltaTime;
            if(RemainingCooldown <= 0)
            {
                AddCharge();
            }
        }
        if(RemainingCastDelay > 0)
        {
            RemainingCastDelay -= Time.deltaTime;
        }
    }
    // Called on all clients after server validation
    public virtual void ClientCastAbility(Vector2 mousePos)
    {
        if (!identity.isHost)
        {
            // Exclude hosts so charges are not subtracted twice.
            RemainingCharges--;
        }
        StartCooldown();
        StartCastDelay();
        OnAbilityCast?.Invoke(this);
    }
    public virtual void CastAbility(Vector2 mousePos)
    {
        RemainingCharges--;
        StartCooldown();
        StartCastDelay();
    }
    public void ResetCharges()
    {
        RemainingCharges = maxCharges;
    }
    private void AddCharge()
    {
        RemainingCharges++;
        if(RemainingCharges < maxCharges)
        {
            StartCooldown();
        }
    }
    private void StartCastDelay()
    {
        RemainingCastDelay = baseCastDelay;
    }
    private void StartCooldown()
    {
        if (RemainingCooldown <= 0)
        {
            RemainingCooldown = baseCooldown;
        }
    }
    
}
