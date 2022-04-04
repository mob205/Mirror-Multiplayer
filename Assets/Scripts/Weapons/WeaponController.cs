using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponController : MonoBehaviour
{
    [Tooltip("Fire rate in attacks per second")]
    [SerializeField] protected float fireRate = 1;
    [SerializeField] protected float damage = 10;

    //public float AbilityModifier { get; private set; }

    protected bool canFire = true;
    protected Camera mainCam;

    private void Start()
    {
        mainCam = Camera.main;
    }

    public virtual void RotateWeapon(Vector3 target)
    {
        transform.rotation = Utility.GetDirection(target, transform);
    }
    public abstract bool ServerFire(Vector3 target, ref GameObject go);

    public abstract void SimulateFire(GameObject go, Vector3 target);

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
    //public void ModifyAbilityDamage(float modifier)
    //{
    //    AbilityModifier *= modifier;
    //}
}
