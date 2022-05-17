using System.Collections;
using UnityEngine;

public class MeleeWeapon : WeaponController
{
    [SerializeField] private float attackArcDegrees = 90;
    [Tooltip("Time in seconds to swing one way")]
    [SerializeField] private float swingTime = 1;
    [SerializeField] private MeleeHitbox hitbox;

    private Quaternion swingRot = Quaternion.identity;
    public override bool ServerFire(Vector3 target)
    {
        if (!canFire) { return false; }
        if (!hitbox) { hitbox = GetComponentInChildren<MeleeHitbox>(); }

        // Hit detection should be server-side only.
        hitbox.ClearHitPlayers();
        hitbox.Damage = damage;
        hitbox.Shooter = transform.parent.gameObject;
        hitbox.Weapon = this;

        hitbox.CanDamage = true;

        if (!netIdentity.isHost)
        {
            StartCoroutine(SwingWeapon());
        }
        StartCoroutine(ToggleFire());
        return true;
    }
    public override void SimulateFire(Vector3 target)
    {
        StartCoroutine(SwingWeapon());
        StartCoroutine(ToggleFire());
        base.SimulateFire(target);
    }
    public override void RotateWeapon(Vector3 target)
    {
        base.RotateWeapon(target);
        // Combine Quaternions through multiplication.
        transform.rotation *= swingRot;
    }
    private IEnumerator SwingWeapon()
    {
        var swingSpeed = attackArcDegrees / swingTime;
        for (float angle = 0; angle < attackArcDegrees; angle += swingSpeed * Time.deltaTime)
        {
            swingRot = Quaternion.Euler(new Vector3(0, 0, angle));
            yield return null;
        }
        for(float angle = attackArcDegrees; angle > 0; angle -= swingSpeed * Time.deltaTime)
        {
            swingRot = Quaternion.Euler(new Vector3(0, 0, angle));
            yield return null;
        }

        hitbox.CanDamage = false;
    }
    public void ModifySwingSpeed(float modifier)
    {
        swingTime *= modifier;
    }
}
