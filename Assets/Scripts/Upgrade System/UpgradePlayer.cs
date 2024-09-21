using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradePlayer : NetworkBehaviour
{
    private UpgradeManager upgradeManager;
    private UpgradeUI upgradeUI;

    public override void OnStartAuthority()
    {
        upgradeManager = FindObjectOfType<UpgradeManager>();
        upgradeUI = FindObjectOfType<UpgradeUI>();
        upgradeManager.Initialize();
        upgradeUI.Initialize(upgradeManager);
    }
}
