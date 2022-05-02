using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashAbilityUpgrade : AbilityUpgrade
{
    public float speed;
    public float distance;
    public float damage;

    private PlayerMovement player;
    public override void Initialize()
    {
        base.Initialize();
        player = GetComponent<PlayerMovement>();
    }
    public override void CastAbility(Vector2 mousePos)
    {
        var dirAngle = Utility.GetDirection(mousePos, transform).eulerAngles.z * Mathf.Deg2Rad;
        var directionVector = new Vector2(Mathf.Cos(dirAngle), Mathf.Sin(dirAngle));
        player.SetState(PlayerMovement.State.Dashing, distance/speed);
        player.SetDashVector(directionVector, speed);
        gameObject.layer = LayerMask.NameToLayer("Dashing");
        StartCooldown();
    }
    public override void ClientCastAbility(Vector2 mousePos)
    {
        gameObject.layer = LayerMask.NameToLayer("Dashing");
        base.ClientCastAbility(mousePos);
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (identity.isServer && player.CurrentState == PlayerMovement.State.Dashing && collider.GetComponent<Health>())
        {
            var health = collider.GetComponent<Health>();
            health.Damage(damage, player.gameObject);
        }
    }
}
