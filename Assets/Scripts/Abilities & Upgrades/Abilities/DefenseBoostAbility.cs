using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenseBoostAbility : AbilityUpgrade
{
    public float duration;
    public float resistance;
    public ParticleSystem particlePrefab;

    public override void CastAbility(Vector2 mousePos)
    {
        StartCoroutine(ApplyBuff(GetComponent<Health>()));
        StartCooldown();
    }
    public override void ClientCastAbility(Vector2 mousePos)
    {
        PlayEffects();
        base.ClientCastAbility(mousePos);
    }
    private IEnumerator ApplyBuff(Health health)
    {
        health.DamageResistance += resistance;
        yield return new WaitForSeconds(duration);
        health.DamageResistance -= resistance;
    }
    private void PlayEffects()
    {
        if (!particlePrefab) { return; }
        var particles = Instantiate(particlePrefab, transform.position, Quaternion.identity, transform);
        var particlesMain = particles.main;
        particlesMain.duration = duration;
        particles.Play();
    }
}
