using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisibilityAbility : AbilityUpgrade
{
    public float duration;
    public float casterOpacity;
    public ParticleSystem particles;

    private SpriteRenderer spriteRenderer;
    private PlayerCombat playerCombat;
    private PlayerAbilities playerAbilities;
    private SpriteRenderer weaponRenderer;

    private double targetTime;
    private bool isInvis;
    protected override void Start()
    {
        base.Start();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerCombat = GetComponent<PlayerCombat>();
        playerAbilities = GetComponent<PlayerAbilities>();

        playerCombat.OnFire += StopEffect;
        playerAbilities.OnCast += StopEffect;
    }
    protected override void Update()
    {
        base.Update();
        if (isInvis && NetworkTime.time >= targetTime)
        {
            StopEffect();
        }
    }
    public override void CastAbility(Vector2 mousePos)
    {
        StartCooldown();
    }
    public override void ClientCastAbility()
    {
        weaponRenderer = playerCombat.Weapon.GetComponentInChildren<SpriteRenderer>();

        targetTime = NetworkTime.time + duration;
        if (networkIdentity.isLocalPlayer)
        {
            var transparent = new Color(255, 255, 255, casterOpacity);
            spriteRenderer.color = transparent;
            weaponRenderer.color = transparent;
        }
        else
        {
            spriteRenderer.enabled = false;
            weaponRenderer.enabled = false;
        }
        isInvis = true;

        if (particles)
        {
            Instantiate(particles, transform.position, Quaternion.identity);
        }
        base.ClientCastAbility();
    }
    private void StopEffect()
    {
        if (isInvis)
        {
            var opaque = new Color(255, 255, 255, 1);
            spriteRenderer.color = opaque;
            weaponRenderer.color = opaque;
            spriteRenderer.enabled = true;
            weaponRenderer.enabled = true;
            isInvis = false;
        }
    }
}
