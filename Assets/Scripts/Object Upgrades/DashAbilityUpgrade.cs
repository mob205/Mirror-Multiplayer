using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashAbilityUpgrade : AbilityUpgrade
{
    private Rigidbody2D rb;
    private Camera cam;
    protected override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
        cam = Camera.main; 
    }
    protected override void CastAbility()
    {
        var mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        var dir = Utility.GetDirection(mousePos, transform);
        CmdTest(dir.x);
        StartCooldown();
    }
    [Command]
    private void CmdTest(float x)
    {
        Debug.Log($"received {x}");
    }
}
