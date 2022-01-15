using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradePlayer : NetworkBehaviour
{
    private UpgradeManager upgradeManager;
    public override void OnStartAuthority()
    {
        upgradeManager = FindObjectOfType<UpgradeManager>();
        upgradeManager.Initialize();
    }
}
