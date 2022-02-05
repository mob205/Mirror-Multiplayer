using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilities : NetworkBehaviour
{
    Dictionary<string, AbilityUpgrade> abilities = new Dictionary<string, AbilityUpgrade>();
    public void AddAbility(AbilityUpgrade ability)
    {
        abilities.Add(ability.abilityName, ability);
    }
    [Command]
    public void CmdActivateAbility(string ability)
    {
        abilities[ability].CastAbility();
        //RpcActivateAbility(ability);
    }
    //[ClientRpc]
    //public void RpcActivateAbility(string ability)
    //{
    //    abilities[ability].CastAbility();
    //}
}
