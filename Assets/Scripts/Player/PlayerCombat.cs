using Mirror;
using System;
using System.Collections;
using UnityEngine;

public class PlayerCombat : NetworkBehaviour
{
    public float FacingAngle { get; private set; }
    public WeaponController Weapon { get { return weapon; } }

    private Camera mainCam;
    private WeaponController weapon;
    private PlayerMovement playerMovement;

    public Action OnFire;
    void Awake()
    {
        mainCam = Camera.main;
        weapon = GetComponentInChildren<WeaponController>();
        playerMovement = GetComponent<PlayerMovement>();
    }
    private void Update()
    {
        if (hasAuthority && weapon && playerMovement.CurrentState != PlayerMovement.State.Immobilized)
        {
            var target = mainCam.ScreenToWorldPoint(Input.mousePosition);
            if (NetworkClient.ready)
            {
                CmdRotateWeapon(target);
            }
            if (Input.GetButton("Fire1") && weapon.CanFire)
            {
                CmdFire(target);
            }
        }
    }
    [Command]
    private void CmdFire(Vector3 target)
    {
        if (weapon.ServerFire(target))
        {
            RpcSimulateFire(target);
        }
    }
    [ClientRpc]
    private void RpcSimulateFire(Vector3 target)
    {
        weapon.SimulateFire(target);
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
