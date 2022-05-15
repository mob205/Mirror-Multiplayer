using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ExplosionAbility : AbilityUpgrade
{
    [Header("Particles")]
    public ParticleSystem expParticlePrefab;
    public ParticleSystem startParticlePrefab;
    public LayerMask wallLayers;
    [Header("Gameplay")]
    public ExplosionHitDetection explosionPrefab;
    public float range;
    public float damage;
    public float warmupDuration;
    public override void CastAbility(Vector2 mousePos)
    {
        StartCoroutine(DelayedCalculateDamage());
        base.CastAbility(mousePos);
    }
    public override void ClientCastAbility(Vector2 mousePos)
    {
        StartCoroutine(PlayEffects());
        base.ClientCastAbility(mousePos);
    }
    private IEnumerator PlayEffects()
    {
        PlayChargeParticles();
        yield return new WaitForSeconds(warmupDuration);
        Instantiate(expParticlePrefab, transform.position, Quaternion.identity);
    }
    private void PlayChargeParticles()
    {
        var startParticles = Instantiate(startParticlePrefab, transform.position, Quaternion.identity, transform);
        var startMain = startParticles.main;

        // time = distance / speed 
        var lifetime = startParticles.shape.radius / Mathf.Abs(startMain.startSpeed.constant);
        startMain.startLifetime = lifetime;

        // warmupDuration (total playtime of system) = duration of spawning particles + lifetime of lingering particles
        startMain.duration = warmupDuration - lifetime;
        startParticles.Play();
    }
    private IEnumerator DelayedCalculateDamage()
    {
        yield return new WaitForSeconds(warmupDuration);
        var explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        explosion.MaxRange = range;
        explosion.Caster = GetComponent<Health>();
        explosion.ExpansionRate = expParticlePrefab.main.startSpeed.constantMax;
        explosion.Damage = damage;
        explosion.Layers = wallLayers;
    }
}
