using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : WeaponController
{
    [SerializeField] private float attackArcDegrees = 90;
    [SerializeField] private float swingTime = 1;
    [SerializeField] private BoxCollider2D hitbox;

    private Quaternion swingRot = Quaternion.identity;
    public override GameObject ServerFire(Vector3 target)
    {
        if (!canFire) { return null; }
        hitbox.enabled = true;
        StartCoroutine(ToggleFire());
        return transform.parent.gameObject;
    }
    public override void SimulateFire(GameObject go, Vector3 target)
    {
        transform.rotation = GetDirection(target);
        StartCoroutine(SwingWeapon());
    }
    public override void RotateWeapon(Vector3 target)
    {
        base.RotateWeapon(target);
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

        hitbox.enabled = false;
    }
}
