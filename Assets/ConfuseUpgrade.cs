using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfuseUpgrade : Upgrade
{
    public float stunDuration;
    private PlayerMovement player;
    public override void Initialize()
    {
        player = GetComponent<PlayerMovement>();
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        var target = collider.GetComponent<PlayerMovement>();
        if (NetworkServer.active && player.CurrentState == PlayerMovement.State.Dashing && target)
        {
            target.SetState(PlayerMovement.State.Immobilized, stunDuration);
        }
    }
}
