using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashAbilityUpgrade : AbilityUpgrade
{
    public float speed;
    public float distance;

    private Rigidbody2D rb;
    private Camera cam;
    private PlayerMovement player;
    protected override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
        player = GetComponent<PlayerMovement>();
        cam = Camera.main; 
    }
    public override void CastAbility()
    {
        var mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        var dirAngle = Utility.GetDirection(mousePos, transform).eulerAngles.z * Mathf.Deg2Rad;
        var directionVector = new Vector2(Mathf.Cos(dirAngle), Mathf.Sin(dirAngle));
        Debug.Log($"Angle: {dirAngle} | Coordinates: ({directionVector.x}, {directionVector.y})");
        player.Dash(directionVector, speed, distance/speed);
    }
}
