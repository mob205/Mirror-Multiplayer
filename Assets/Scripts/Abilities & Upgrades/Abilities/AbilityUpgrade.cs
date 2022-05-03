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
    public float castDelay;

    public float RemainingCooldown { get; private set; } = 0;
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
        abilities.AddAbility(this);

        if (identity.isLocalPlayer) 
        {
            cam = Camera.main; 
        }
    }
    protected virtual void Update()
    {
        UpdateCooldown();
        if (RemainingCooldown <= 0 && identity.isLocalPlayer && Input.GetButton($"Ability{OrderNumber}") && playerMovement.CurrentState != PlayerMovement.State.Immobilized)
        {
            var mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
            abilities.RequestAbility(abilityID, mousePos);
        }
    }
    private void UpdateCooldown()
    {
        if (RemainingCooldown >= 0)
        {
            RemainingCooldown -= Time.deltaTime;
        }
    }
    // Called on all clients after server validation
    public virtual void ClientCastAbility(Vector2 mousePos)
    {
        StartCooldown();
        OnAbilityCast?.Invoke(this);
    }
    public virtual void CastAbility(Vector2 mousePos)
    {
        StartCooldown();
    }
    protected void StartCooldown()
    {
        RemainingCooldown = baseCooldown;
    }
}
