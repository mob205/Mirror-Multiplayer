using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbilityUpgrade : MonoBehaviour
{
    public Sprite icon;
    public float baseCooldown;

    public float RemainingCooldown { get; private set; }
    public int OrderNumber { get; set; }

    private AbilityUI abilityUI;

    private void Start()
    {
        abilityUI = FindObjectOfType<AbilityUI>();
        abilityUI.AddAbility(this);
    }
    protected virtual void Update()
    {
        UpdateCooldown();
        if (RemainingCooldown <= 0 && Input.GetButton($"Ability{OrderNumber}"))
        {
            CastAbility();
        }
    }
    private void UpdateCooldown()
    {
        if (RemainingCooldown >= 0)
        {
            RemainingCooldown -= Time.deltaTime;
        }
    }
    protected abstract void CastAbility();
    protected void StartCooldown()
    {
        RemainingCooldown = baseCooldown;
    }
}
