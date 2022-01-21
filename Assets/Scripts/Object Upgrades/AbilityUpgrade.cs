using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbilityUpgrade : NetworkBehaviour
{
    public Sprite icon;
    public float baseCooldown;

    public float RemainingCooldown { get; private set; } = 0;
    public int OrderNumber { get; set; }

    private AbilityUI abilityUI;
    private NetworkIdentity identity;

    protected virtual void Start()
    {
        identity = GetComponent<NetworkIdentity>();
        if (!identity.isLocalPlayer) { return; }
        abilityUI = FindObjectOfType<AbilityUI>();
        abilityUI.AddAbility(this);
    }
    protected virtual void Update()
    {
        if (!identity.isLocalPlayer) { return; }
        UpdateCooldown();
        if (RemainingCooldown <= 0 && Input.GetButton($"Ability{OrderNumber}"))
        {
            CastAbility();
        }
    }
    private void UpdateCooldown()
    {
        if (RemainingCooldown >= 0)
        {
            RemainingCooldown -= Time.deltaTime;
        }
    }
    protected abstract void CastAbility();
    protected void StartCooldown()
    {
        RemainingCooldown = baseCooldown;
    }
}
