using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowOnHitUpgrade : Upgrade
{
    public float speedModifier;
    public float slowDuration;
    private Dictionary<PlayerMovement, float> slowedPlayers = new Dictionary<PlayerMovement, float>();
    public override void Initialize()
    {
        GetComponentInChildren<WeaponController>().OnHit += OnHit;
    }
    private void Update()
    {
        var currentSlowed = new Dictionary<PlayerMovement, float>(slowedPlayers);
        foreach(var player in currentSlowed)
        {
            if(player.Value <= 0)
            {
                slowedPlayers.Remove(player.Key);
                player.Key.speedModifier /= speedModifier;
            }
            else
            {
                slowedPlayers[player.Key] -= Time.deltaTime;
            }
        }
    }
    private void OnHit(Health health)
    {
        var player = health.GetComponent<PlayerMovement>();
        if (slowedPlayers.ContainsKey(player))
        {
            slowedPlayers[player] = slowDuration;
        }
        else
        {
            slowedPlayers.Add(player, slowDuration);
            player.speedModifier *= speedModifier;
            Debug.Log("Player slowed");
        }
    }
    private void OnDestroy()
    {
        foreach (var player in slowedPlayers)
        {
            slowedPlayers.Remove(player.Key);
            player.Key.speedModifier /= speedModifier;
        }
    }
}
