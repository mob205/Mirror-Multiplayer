using Mirror;
using System.Collections;
using UnityEngine;

public class PlayerCombat : NetworkBehaviour
{
    private Camera mainCam;
    private WeaponController weapon;

    [SyncVar] public int currentWeaponIndex;
    void Awake()
    {
        mainCam = Camera.main;
        weapon = GetComponentInChildren<WeaponController>();
    }
    private void Update()
    {
        if (hasAuthority && weapon)
        {
            var target = mainCam.ScreenToWorldPoint(Input.mousePosition);
            CmdRotateWeapon(target);
            if (Input.GetButtonDown("Fire1"))
            {
                CmdFire(target);
            }
        }
    }
    [Command]
    private void CmdFire(Vector3 target)
    {
        GameObject go = null;
        if (weapon.ServerFire(target, ref go))
        {
            RpcSimulateFire(go, target);
        }
    }
    [ClientRpc]
    private void RpcSimulateFire(GameObject go, Vector3 target)
    {
        weapon.SimulateFire(go, target);
    }
    // In the future, rotation may happen in the parent player object, not the weapon.
    // Only z coordinate for rotation needs to pass through the network, not the entire Vector3.
    [Command]
    private void CmdRotateWeapon(Vector3 target)
    {
        RpcRotateWeapon(target);
    }
    [ClientRpc]
    private void RpcRotateWeapon(Vector3 target)
    {
        if (weapon)
        {
            weapon.RotateWeapon(target);
        }
    }
    // To be called locally by a Class Upgrade
    public void SetWeapon(GameObject weaponObj)
    {
        weapon = Instantiate(weaponObj, transform.position, Quaternion.identity, transform).GetComponent<WeaponController>();
    }
}
