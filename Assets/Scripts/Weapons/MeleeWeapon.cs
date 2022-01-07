using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : WeaponController
{
    [SerializeField] private float attackArcDegrees = 90;
    [SerializeField] private float swingTime = 1;
    [SerializeField] private BoxCollider2D hitbox;

    private bool isAttacking = false;
    
    public override GameObject ServerFire(Vector3 target)
    {
        if (!canFire || isAttacking) { return null; }
        hitbox.enabled = true;
        //SimulateFire(gameObject, target); 
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
        if (!isAttacking)
        {
            base.RotateWeapon(target);
        }
    }
    private IEnumerator SwingWeapon()
    {
        isAttacking = true;
        var swingSpeed = attackArcDegrees / swingTime;
        var swingDistance = 0f;
        while(swingDistance < attackArcDegrees)
        {
            var angle = swingSpeed * Time.deltaTime;
            swingDistance += angle;
            transform.Rotate(new Vector3(0, 0, angle));
            yield return null;
        }
        // Disable hitbox after initial swing. The recovery should not damage
        while (swingDistance > 0)
        {
            var angle = -swingSpeed * Time.deltaTime;
            swingDistance += angle;
            transform.Rotate(new Vector3(0, 0, angle));
            yield return null;
        }
        hitbox.enabled = false;
        isAttacking = false;
    }
}
