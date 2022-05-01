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
            GameSceneManager.OnPlayerWin += AwardBonus;
        }
    }
    private void AwardBonus(uint winner)
    {
        var identity = GetComponent<NetworkIdentity>();
        var tracker = FindObjectOfType<PointTracker>();
        if(winner == identity.netId)
        {
            Debug.Log("BONUS DUCKS!");
            var conn = NetworkServer.spawned[winner].connectionToClient;
            tracker.AddPoints(conn, bonusPoints);
        }
    }
    private void OnDestroy()
    {
        if (NetworkServer.active)
        {
            GameSceneManager.OnPlayerWin -= AwardBonus;
        }
    }
}
