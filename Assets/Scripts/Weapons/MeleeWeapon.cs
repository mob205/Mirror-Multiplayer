using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : WeaponController
{
    [SerializeField] private float attackArcDegrees = 90;
    [Tooltip("Time in seconds to swing one way")]
    [SerializeField] private float swingTime = 1;
    [SerializeField] private GameObject hitbox;

    private Quaternion swingRot = Quaternion.identity;
    public override bool ServerFire(Vector3 target, ref GameObject go)
    {
        if (!canFire) { return false; }

        // Hit detection should be server-side only.
        hitbox.SetActive(true);
        var hitScript = hitbox.GetComponent<MeleeHitbox>();
        hitScript.Damage = damage;
        hitScript.Shooter = transform.parent.gameObject;

        StartCoroutine(ToggleFire());
        return true;
    }
    public override void SimulateFire(GameObject go, Vector3 target)
    {
        transform.rotation = GetDirection(target);
        StartCoroutine(SwingWeapon());
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

        hitbox.SetActive(false);
    }
}
