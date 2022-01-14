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
    public void Start()
    {
        // Newly joining clients set and display the weapons of already joined players
        if (!weapon)
        {
            SetWeapon(currentWeaponIndex);
        }
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
    [Command]
    private void CmdRotateWeapon(Vector3 target)
    {
        RpcRotateWeapon(target);
    }
    [ClientRpc]
    private void RpcRotateWeapon(Vector3 target)
    {
        if (!weapon)
        {
            return;
        }
        weapon.RotateWeapon(target);
    }
    [Command(requiresAuthority = false)]
    public void CmdSetWeapon(int weaponIndex)
    {
        currentWeaponIndex = weaponIndex;
        RpcSetWeapon(currentWeaponIndex);
    }
    [ClientRpc]
    private void RpcSetWeapon(int weaponIndex)
    {
        SetWeapon(weaponIndex);
    }
    [Client]
    private void SetWeapon(int weaponIndex)
    {
        if (weapon)
        {
            // Problem where Start() will run before the RPC can run SetWeapon, causing an inactive weapon to spawn visually on other clients.
            return;
        }
        var weaponObj = Instantiate(CustomNetworkManager.singleton.weapons[weaponIndex], transform.position, Quaternion.identity, transform);
        weapon = weaponObj.GetComponent<WeaponController>();
    }
}
