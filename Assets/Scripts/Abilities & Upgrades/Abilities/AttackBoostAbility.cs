using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBoostAbility : AbilityUpgrade
{
    [Header("Gameplay")]
    public float duration;
    public float attackModifier;
    public float firerateModifier;

    public ParticleSystem particlePrefab;

    public override void CastAbility(Vector2 mousePos)
    {
        StartCoroutine(ApplyBuff());
        base.CastAbility(mousePos);
    }
    public override void ClientCastAbility(Vector2 mousePos)
    {
        StartCoroutine(ApplyBuff());
        PlayEffects();
        base.ClientCastAbility(mousePos);
    }
    private IEnumerator ApplyBuff()
    {
        var weapon = GetComponent<PlayerCombat>().Weapon;

        weapon.ModifyDamage(attackModifier);
        weapon.ModifyFirerate(firerateModifier);
        if(weapon is MeleeWeapon)
        {
            (weapon as MeleeWeapon).ModifySwingSpeed(1 / firerateModifier);
        }

        yield return new WaitForSeconds(duration);

        weapon.ModifyDamage(1 / attackModifier);
        weapon.ModifyFirerate(1 / firerateModifier);
        if (weapon is MeleeWeapon)
        {
            (weapon as MeleeWeapon).ModifySwingSpeed(firerateModifier);
        }
    }
    private void PlayEffects()
    {
        if (!particlePrefab) { return; }
        var weaponSprite = GetComponent<PlayerCombat>().Weapon.GetComponentInChildren<SpriteRenderer>();
        var particles = Instantiate(particlePrefab, weaponSprite.transform.position, weaponSprite.transform.rotation, weaponSprite.transform);
        var particlesMain = particles.main;
        particlesMain.duration = duration;
        particles.Play();
    }
}
