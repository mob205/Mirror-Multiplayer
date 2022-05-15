using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RampupBullet : Bullet
{
    public float damageGainRate;
    public float updatesPerSecond;

    private float startTime;
    private float baseDamage;
    private void Start()
    {
        baseDamage = Damage;
        startTime = Time.time;
        StartCoroutine(UpdateDamage());
    }
    private IEnumerator UpdateDamage()
    {
        while (this)
        {
            Damage = damageGainRate * Mathf.Sqrt(Time.time - startTime) + baseDamage;
            yield return new WaitForSeconds(1 / updatesPerSecond);
        }
    }
    public void OnDestroy()
    {
        Debug.Log($"{startTime}, {Damage}");
    }
}
