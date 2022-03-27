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
    }
}
