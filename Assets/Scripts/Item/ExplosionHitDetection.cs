using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionHitDetection : MonoBehaviour
{
    public float ExpansionRate { get; set; }
    public float MaxRange { get; set; }
    public Health Caster { get; set; }
    public float Damage { get; set; }
    public LayerMask Layers { get; set; }

    private CircleCollider2D expCollider;
    private float currentRadius;
    private void Start()
    {
        expCollider = GetComponent<CircleCollider2D>();
        currentRadius = expCollider.radius;
    }
    private void OnTriggerEnter2D(Collider2D target)
    {
        var targetHealth = target.GetComponent<Health>();
        if (targetHealth && targetHealth != Caster && !Physics2D.Linecast(transform.position, target.transform.position, Layers))
        {
            targetHealth.Damage(Damage, Caster.gameObject);
        }
    }
    private void Update()
    {
        if(currentRadius < MaxRange)
        {
            currentRadius += ExpansionRate * Time.deltaTime;
            expCollider.radius = currentRadius;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
