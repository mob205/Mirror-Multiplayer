using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponController : MonoBehaviour
{
    [Tooltip("Fire rate in attacks per second")]
    [SerializeField] protected float fireRate = 1;
    [SerializeField] protected float damage = 10;
    [SerializeField] protected AudioSource sound;

    public bool CanFire { get { return canFire; } }

    protected bool canFire = true;
    protected Camera mainCam;
    protected NetworkIdentity netIdentity;

    public event Action<Health> OnHit;

    private void Start()
    {
        mainCam = Camera.main;
        netIdentity = GetComponentInParent<NetworkIdentity>();
    }

    public virtual void RotateWeapon(Vector3 target)
    {
        transform.rotation = Utility.GetDirection(target, transform);
    }
    public abstract bool ServerFire(Vector3 target);

    public virtual void SimulateFire(Vector3 target)
    {
        if (sound)
        {
            sound.pitch = UnityEngine.Random.Range(.9f, 1.1f);
            sound.Play();
        }
    }

    protected IEnumerator ToggleFire()
    {
        canFire = false;
        yield return new WaitForSeconds(1 / fireRate);
        canFire = true;
    }
    public void ModifyDamage(float modifier)
    {
        damage *= modifier;
    }
    public void ModifyFirerate(float modifier)
    {
        fireRate *= modifier;
    }
    public void TriggerHit(Health target)
    {
        OnHit?.Invoke(target);
    }
}
