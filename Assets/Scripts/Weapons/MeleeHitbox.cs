using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeHitbox : MonoBehaviour
{
    public float Damage { get; set; }
    public GameObject Shooter { get; set; }

    public bool CanDamage { get; set; }

    public event Action<Health> OnHit;

    protected List<Health> damaged = new List<Health>();
    public void ClearHitPlayers()
    {
        damaged.Clear();
    }
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (!CanDamage) { return; }
        var target = collision.GetComponent<Health>();
        if (target && !damaged.Contains(target) && collision.gameObject != Shooter)
        {
            DamageTarget(target, Damage);
            Debug.Log($"Damaging {collision.name}");
        }
    }
    protected virtual void DamageTarget(Health target, float damage)
    {
        target.Damage(damage, Shooter);
        damaged.Add(target);
        OnHit?.Invoke(target);
    }
}

