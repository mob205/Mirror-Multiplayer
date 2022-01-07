using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponController : MonoBehaviour
{
    [SerializeField] protected float fireRate = 1;
    [SerializeField] protected float damage = 10;

    protected bool canFire = true;
    protected Camera mainCam;

    private void Start()
    {
        mainCam = Camera.main;
    }

    public virtual void ClientFire()
    {

    }

    public virtual GameObject ServerFire(Vector3 target)
    {
        return null;
    }

    public virtual void SimulateFire(GameObject go, Vector3 target)
    {

    }

    protected IEnumerator ToggleFire()
    {
        canFire = false;
        yield return new WaitForSeconds(1 / fireRate);
        canFire = true;
    }
}
