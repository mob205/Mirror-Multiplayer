using Mirror;
using UnityEngine;

public class ThiefUpgrade : Upgrade
{
    public float damageModPerCoin;
    public float stealMin;
    public float stealMax;

    private NetworkIdentity identity;
    public override void Initialize()
    {
        identity = GetComponent<NetworkIdentity>();
        if (!identity.isServer) { return; }

        var damageBoost = 1 + damageModPerCoin * CoinManager.instance.GetCoins(identity.connectionToClient);
        var weapon = GetComponentInChildren<WeaponController>();
        if (weapon)
        {
            weapon.ModifyDamage(damageBoost);
        }
        PlayerHealth.OnPlayerDeath += StealCoins;
    }
    private void StealCoins(Health killedTarget, uint killerID)
    {
        if (identity.isServer && killerID == identity.netId)
        {
            var targetConn = killedTarget.GetComponent<NetworkIdentity>().connectionToClient;
            var killerConn = identity.connectionToClient;
            var randomPercent = Random.Range(stealMin, stealMax);
            var stealAmount = Mathf.FloorToInt(CoinManager.instance.GetCoins(targetConn) * randomPercent);

            CoinManager.instance.ModifyCoins(targetConn, -stealAmount);
            CoinManager.instance.ModifyCoins(killerConn, stealAmount);
        }
    }
    private void OnDestroy()
    {
        if (identity.isServer)
        {
            PlayerHealth.OnPlayerDeath -= StealCoins;
        }
    }
}
