using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtraPointsUpgrade : Upgrade
{
    public int bonusPoints;

    public override void Initialize()
    {
        if (NetworkServer.active)
        {
            GameSceneManager.OnWinRound += AwardBonus;
        }
    }
    private void AwardBonus(uint winner)
    {
        var identity = GetComponent<NetworkIdentity>();
        var tracker = FindObjectOfType<PointTracker>();
        if(winner == identity.netId)
        {
            var conn = NetworkServer.spawned[winner].connectionToClient;
            tracker.AddPoints(conn, bonusPoints);
        }
    }
    private void OnDestroy()
    {
        if (NetworkServer.active)
        {
            GameSceneManager.OnWinRound -= AwardBonus;
        }
    }
}
