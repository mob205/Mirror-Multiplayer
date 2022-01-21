using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeHitbox : MonoBehaviour
{
    public float Damage { get; set; }
    public GameObject Shooter { get; set; }

    private List<Health> damaged = new List<Health>();
    public void ClearHitPlayers()
    {
        damaged.Clear();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var target = collision.GetComponent<Health>();
        if(target && !damaged.Contains(target) && collision.gameObject != Shooter)
        {
            target.Damage(Damage);
            damaged.Add(target);
        }
    }
}

