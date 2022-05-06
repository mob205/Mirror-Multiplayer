using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameReset : NetworkBehaviour
{
    public override void OnStartServer()
    {
        CoinManager.ResetStatics();
        UpgradeManager.ResetStatics();
        PointTracker.ResetStatics();
    }
    public override void OnStartClient()
    {
        CoinManager.ClientCoins = 0;
    }
}
