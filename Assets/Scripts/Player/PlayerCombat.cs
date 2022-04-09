using Mirror;
using System;
using System.Collections;
using UnityEngine;

public class PlayerCombat : NetworkBehaviour
{
    public float FacingAngle { get; private set; }
    private Camera mainCam;
    private WeaponController weapon;
    public WeaponController Weapon { get { return weapon; } }

    public Action OnFire;
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
            if (connectionToServer.isReady)
            {
                CmdRotateWeapon(target);
            }
            if (Input.GetButton("Fire1"))
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
        OnFire?.Invoke();
    }
    // In the future, rotation may happen in the parent player object, not the weapon.
    // Only angle of rotation needs to pass through the network, not the entire Vector3.
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
        FacingAngle = Utility.GetDirection(target, transform).eulerAngles.z;
    }
    // To be called locally by a Class Upgrade
    public void SetWeapon(GameObject weaponObj)
    {
        weapon = Instantiate(weaponObj, transform.position, Quaternion.identity, transform).GetComponent<WeaponController>();
    }
}
