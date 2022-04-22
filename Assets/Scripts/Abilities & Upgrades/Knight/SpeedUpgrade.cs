using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedUpgrade : Upgrade
{
    public float speedModifier;
    private PlayerMovement player;
    public override void Initialize()
    {
        player = GetComponent<PlayerMovement>();
        player.speedModifier *= speedModifier;
    }
}
