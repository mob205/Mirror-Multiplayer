using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilities : NetworkBehaviour
{
    private Dictionary<string, AbilityUpgrade> abilities = new Dictionary<string, AbilityUpgrade>();
    private AbilityUI abilityUI;

    public override void OnStartAuthority()
    {
        abilityUI = FindObjectOfType<AbilityUI>();
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
        if(abilities[ability].RemainingCooldown <= 0)
        {
            abilities[ability].CastAbility(mousePos);
            OnSuccessfulCast(sender, ability);
        }
    }
    public void RequestAbility(string ability, Vector2 mousePos)
    {
        CmdCastAbility(ability, mousePos);
    }
    [TargetRpc]
    private void OnSuccessfulCast(NetworkConnection target, string ability)
    {
        abilities[ability].OnSuccessfulCast();
    }
    //[ClientRpc]
    //public void RpcActivateAbility(string ability)
    //{
    //    abilities[ability].CastAbility();
    //}
}
