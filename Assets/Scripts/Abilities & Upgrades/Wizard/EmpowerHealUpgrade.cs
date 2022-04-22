using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmpowerHealUpgrade : Upgrade
{
    public float damageModifier;
    public float speedModifier;
    public float buffDuration;

    private NetworkIdentity identity;
    private PlayerCombat combat;
    private PlayerMovement movement;
    public override void Initialize()
    {
        identity = GetComponent<NetworkIdentity>();
        combat = GetComponent<PlayerCombat>();
        movement = GetComponent<PlayerMovement>();
        GetComponent<AbilityUpgrade>().OnAbilityCast += OnCastHeal;
    }
    private void OnCastHeal(AbilityUpgrade ability)
    {
        if(identity.isServer && ability is HealAbility)
        {
            StartCoroutine(ApplyBuff());
        }
    }
    private IEnumerator ApplyBuff()
    {
        Debug.Log("Applying buff.");
        combat.Weapon.ModifyDamage(damageModifier);
        movement.speedModifier *= speedModifier;
        yield return new WaitForSeconds(buffDuration);
        Debug.Log("Getting rid of buff");
        combat.Weapon.ModifyDamage(1 / damageModifier);
        movement.speedModifier /= speedModifier;
    }
}
