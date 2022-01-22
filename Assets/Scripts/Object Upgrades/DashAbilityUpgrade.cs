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
    private Camera cam;
    private PlayerMovement player;
    protected override void Start()
    {
        base.Start();
        player = GetComponent<PlayerMovement>();
        cam = Camera.main; 
    }
    public override void CastAbility()
    {
        var mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        var dirAngle = Utility.GetDirection(mousePos, transform).eulerAngles.z * Mathf.Deg2Rad;
        var directionVector = new Vector2(Mathf.Cos(dirAngle), Mathf.Sin(dirAngle));
        player.StartDash(directionVector, speed, distance/speed);
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (networkIdentity.isServer && player.CurrentState == PlayerMovement.State.Dashing && collider.GetComponent<Health>())
        {
            var health = collider.GetComponent<Health>();
            health.Damage(damage);
        }
    }
}
