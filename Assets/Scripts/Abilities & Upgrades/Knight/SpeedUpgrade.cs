using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedUpgrade : MonoBehaviour
{
    public float speedModifier;
    private PlayerMovement player;
    void Start()
    {
        player = GetComponent<PlayerMovement>();
        player.speedModifier *= speedModifier;
    }
}
