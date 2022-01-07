using Mirror;
using System.Collections;
using UnityEngine;

public class PlayerCombat : NetworkBehaviour
{ 
    private Camera mainCam;
    private WeaponController weapon;
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
        var go = weapon.ServerFire(target);
        if (go)
        {
            RpcSimulateFire(go, target);
        }
    }
    [ClientRpc]
    private void RpcSimulateFire(GameObject go, Vector3 target)
    {
        weapon.SimulateFire(go, target);
    }
    [Command]
    private void CmdRotateWeapon(Vector3 target)
    {
        RpcRotateWeapon(target);
    }
    [ClientRpc]
    private void RpcRotateWeapon(Vector3 target)
    {
        weapon.RotateWeapon(target);
    }
}
