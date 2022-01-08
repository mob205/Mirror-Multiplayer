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
    public virtual void RotateWeapon(Vector3 target)
    {
        transform.rotation = GetDirection(target);
    }
    public virtual bool ServerFire(Vector3 target, ref GameObject go)
    {
        return false;
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
    protected Quaternion GetDirection(Vector3 target)
    {
        // Get displacement vector components from player object to target
        var y = target.y - transform.position.y;
        var x = target.x - transform.position.x;

        // Get rotation from the arctangent of displacement components
        float angle = Mathf.Atan2(y, x) * Mathf.Rad2Deg;
        return Quaternion.Euler(new Vector3(0, 0, angle));
    }
}
