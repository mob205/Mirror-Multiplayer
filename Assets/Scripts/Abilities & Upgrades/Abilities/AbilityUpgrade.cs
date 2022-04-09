using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbilityUpgrade : MonoBehaviour
{
    public Sprite icon;
    public float baseCooldown;
    public string abilityID;

    public float RemainingCooldown { get; private set; } = 0;
    public int OrderNumber { get; set; }

    protected NetworkIdentity networkIdentity;
    private PlayerAbilities abilities;
    protected Camera cam;

    protected virtual void Start()
    {
        networkIdentity = GetComponent<NetworkIdentity>();
        abilities = GetComponent<PlayerAbilities>();
        abilities.AddAbility(this);

        if (networkIdentity.isLocalPlayer) 
        {
            cam = Camera.main; 
        }
    }
    protected virtual void Update()
    {
        UpdateCooldown();
        if (RemainingCooldown <= 0 && networkIdentity.isLocalPlayer && Input.GetButton($"Ability{OrderNumber}"))
        {
            var mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
            abilities.RequestAbility(abilityID, mousePos);
        }
    }
    private void UpdateCooldown()
    {
        if (RemainingCooldown >= 0)
        {
            RemainingCooldown -= Time.deltaTime;
        }
    }
    // Called on all clients after server validation
    public virtual void ClientCastAbility()
    {
        Debug.Log("An ability was cast.");
    }
    // Called on caster client after server validation
    public virtual void OnSuccessfulCast()
    {
        StartCooldown();
    }
    public abstract void CastAbility(Vector2 mousePos);
    protected void StartCooldown()
    {
        RemainingCooldown = baseCooldown;
    }
}
