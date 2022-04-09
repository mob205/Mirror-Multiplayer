using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class OneshotParticle : MonoBehaviour
{
    private ParticleSystem particles;
    private void Start()
    {
        particles = GetComponent<ParticleSystem>();
    }
    private void Update()
    {
        if (particles.isStopped)
        {
            Destroy(gameObject);
        }
    }
}
