using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinBonusUpgrade : Upgrade
{
    public float percentBonus;

    private NetworkIdentity identity;
    public override void Initialize()
    {
        if (NetworkServer.active)
        {
            identity = GetComponent<NetworkIdentity>();
            GameSceneManager.OnPlayerWin += OnWin;
        }
    }
    private void OnWin(uint winnerID)
    {
        if(identity.netId == winnerID)
        {
            var bonusAmount = Mathf.FloorToInt(GameSceneManager.instance.WinReward * (percentBonus / 100));
            CoinManager.instance.ModifyCoins(identity.connectionToClient, bonusAmount);
        }
        GameSceneManager.OnPlayerWin -= OnWin;
    }
}
