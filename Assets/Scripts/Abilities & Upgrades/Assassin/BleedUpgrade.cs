using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BleedUpgrade : Upgrade
{
    [Header("Activation")]
    public float cooldown;
    public float resetDelay;
    public int hitsNeeded;
    [Header("Damage")]
    public float totalDamage;
    public float TPS;
    public float duration;

    private Dictionary<Health, int> hitCount = new Dictionary<Health, int>();
    private Dictionary<Health, double> timeLastHit = new Dictionary<Health, double>();
    private float currentCD;


    public override void Initialize()
    {
        GetComponentInChildren<MeleeHitbox>().OnHit += OnHit;
    }
    private void Update()
    {
        if(currentCD > 0)
        {
            currentCD -= Time.deltaTime;
        }
    }
    private void OnHit(Health target)
    {
        if(currentCD > 0) { return; }
        if (!hitCount.ContainsKey(target))
        {
            // First time hitting target
            hitCount.Add(target, 1);
            timeLastHit.Add(target, NetworkTime.time);
        }
        else if (NetworkTime.time - timeLastHit[target] > resetDelay)
        {
            // Too long since last hit - reset.
            hitCount[target] = 1;
            timeLastHit[target] = NetworkTime.time;
        }
        else if (hitCount[target] == hitsNeeded - 1)
        {
            hitCount.Clear();
            timeLastHit.Clear();
            StartCoroutine(Activate(target));
        }
        else
        {
            hitCount[target]++;
            timeLastHit[target] = NetworkTime.time;
        }
    }
    private IEnumerator Activate(Health target)
    {
        var damageDealt = 0f;
        var damagePerTick = totalDamage / (TPS * duration);
        while(damageDealt < totalDamage)
        {
            if (!target) { break; }
            target.Damage(damagePerTick, gameObject);
            damageDealt += damagePerTick;
            yield return new WaitForSeconds(1/TPS);
        }
    }
    //private void Activate(Health target)
    //{
    //    target.Damage(damage, gameObject);
    //}
}
