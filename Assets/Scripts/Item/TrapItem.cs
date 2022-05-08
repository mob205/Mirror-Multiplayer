using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapItem : Item
{
    [SyncVar]
    [HideInInspector] public GameObject caster;

    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private ParticleSystem particles;
    [Header("Stats")]
    [SerializeField] private float invisDelay;
    [SerializeField] private float minDamage;
    [SerializeField] private float maxDamage;

    public float itemLifetime;

    private bool isTriggerable;
    protected override void Start()
    {
        base.Start();
        StartCoroutine(StartInvisibility());
        StartCoroutine(StartLifetime());
    }
   

    private IEnumerator StartInvisibility()
    {
        yield return new WaitForSeconds(invisDelay);
        if (!caster.GetComponent<NetworkIdentity>().isLocalPlayer)
        {
            spriteRenderer.enabled = false;
        }
        isTriggerable = true;
    }
    protected override bool IsTriggerable(Collider2D collision)
    {
        if (collision.gameObject == caster)
        {
            return false;
        }
        return isTriggerable;
    }
    [ClientRpc]
    protected override void RpcLocalActivate(GameObject collisionGO)
    {
        Instantiate(particles, transform.position, Quaternion.identity);
    }

    protected override void ServerActivate(Collider2D collision)
    {
        var health = collision.GetComponent<Health>();
        var amount = Random.Range(minDamage, maxDamage + 1);
        health.Damage(amount, caster);
    }
    private IEnumerator StartLifetime()
    {
        yield return new WaitForSeconds(itemLifetime);
        Destroy(gameObject);
    }
}
