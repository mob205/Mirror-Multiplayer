using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbilityUpgrade : MonoBehaviour
{
    public Sprite icon;
    public float baseCooldown;
    public string abilityName;

    public float RemainingCooldown { get; private set; } = 0;
    public int OrderNumber { get; set; }

    private AbilityUI abilityUI;
    protected NetworkIdentity networkIdentity;

    protected virtual void Start()
    {
        networkIdentity = GetComponent<NetworkIdentity>();
        //networkIdentity.AddAbility(this);
        if (!networkIdentity.isLocalPlayer) { return; }

        abilityUI = FindObjectOfType<AbilityUI>();
        abilityUI.AddAbility(this);
    }
    protected virtual void Update()
    {
        if (!networkIdentity.isLocalPlayer) { return; }
        UpdateCooldown();
        if (RemainingCooldown <= 0 && Input.GetButton($"Ability{OrderNumber}"))
        {
            CastAbility();
            //abilityNetworker.CmdActivateAbility(abilityName);
            StartCooldown();
        }
    }
    private void UpdateCooldown()
    {
        if (RemainingCooldown >= 0)
        {
            RemainingCooldown -= Time.deltaTime;
        }
    }
    public abstract void CastAbility();
    protected void StartCooldown()
    {
        RemainingCooldown = baseCooldown;
    }
}
