using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbilityUpgrade : Upgrade
{
    public Sprite icon;
    public float baseCooldown;
    public string abilityID;

    public float RemainingCooldown { get; private set; } = 0;
    public int OrderNumber { get; set; }

    protected NetworkIdentity identity;
    private PlayerAbilities abilities;
    protected Camera cam;

    public Action<AbilityUpgrade> OnAbilityCast;

    public override void Initialize()
    {
        identity = GetComponent<NetworkIdentity>();
        abilities = GetComponent<PlayerAbilities>();
        abilities.AddAbility(this);

        if (identity.isLocalPlayer) 
        {
            cam = Camera.main; 
        }
    }
    protected virtual void Update()
    {
        UpdateCooldown();
        if (RemainingCooldown <= 0 && identity.isLocalPlayer && Input.GetButton($"Ability{OrderNumber}"))
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
        OnAbilityCast?.Invoke(this);
    }
    // Called on caster client after server validation
    public virtual void OnSuccessfulCast()
    {
        StartCooldown();
    }
    public abstract void CastAbility(Vector2 mousePos);
    protected void StartCooldown()
    {
        RemainingCooldown = baseCooldown;
    }
}
