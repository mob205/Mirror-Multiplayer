using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashAbilityUpgrade : AbilityUpgrade
{
    public float speed;
    public float distance;
    public float damage;

    private Rigidbody2D rb;
    private PlayerMovement player;
    protected override void Start()
    {
        base.Start();
        player = GetComponent<PlayerMovement>();
    }
    public override void CastAbility(Vector2 mousePos)
    {
        var dirAngle = Utility.GetDirection(mousePos, transform).eulerAngles.z * Mathf.Deg2Rad;
        var directionVector = new Vector2(Mathf.Cos(dirAngle), Mathf.Sin(dirAngle));
        player.StartDash(directionVector, speed, distance/speed);
        StartCooldown();
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (networkIdentity.isServer && player.CurrentState == PlayerMovement.State.Dashing && collider.GetComponent<Health>())
        {
            var health = collider.GetComponent<Health>();
            health.Damage(damage, player.gameObject);
        }
    }
}