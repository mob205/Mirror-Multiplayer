using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilities : NetworkBehaviour
{
    private Dictionary<string, AbilityUpgrade> abilities = new Dictionary<string, AbilityUpgrade>();
    private AbilityUI abilityUI;
    private PlayerMovement player;

    public Action OnCast;

    public override void OnStartAuthority()
    {
        abilityUI = FindObjectOfType<AbilityUI>();
    }
    public override void OnStartServer()
    {
        player = GetComponent<PlayerMovement>();
    }
    public void AddAbility(AbilityUpgrade ability)
    {
        abilities.Add(ability.abilityID, ability);
        if (abilityUI)
        {
            abilityUI.AddAbility(ability);
        }
    }
    [Command]
    public void CmdCastAbility(string ability, Vector2 mousePos, NetworkConnectionToClient sender = null)
    {
        if(abilities[ability].RemainingCooldown <= 0 && player.CurrentState != PlayerMovement.State.Immobilized)
        {
            abilities[ability].CastAbility(mousePos);
            RpcActivateAbility(ability, mousePos);
        }
    }
    public void RequestAbility(string ability, Vector2 mousePos)
    {
        CmdCastAbility(ability, mousePos);
    }
    [ClientRpc]
    public void RpcActivateAbility(string ability, Vector2 mousePos)
    {
        OnCast?.Invoke();
        abilities[ability].ClientCastAbility(mousePos);
    }
}
