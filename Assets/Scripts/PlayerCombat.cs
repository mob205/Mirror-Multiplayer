using Mirror;
using System.Collections;
using UnityEngine;

public class PlayerCombat : NetworkBehaviour
{ 
    private Camera mainCam;
    private WeaponController weapon;
    void Start()
    {
        mainCam = Camera.main;
        weapon = GetComponentInChildren<WeaponController>();
    }
    private void Update()
    {
        if (hasAuthority && Input.GetButton("Fire1"))
        {
            if (weapon)
            {
                CmdFire(mainCam.ScreenToWorldPoint(Input.mousePosition));
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
}
