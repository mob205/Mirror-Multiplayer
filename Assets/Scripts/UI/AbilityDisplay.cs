using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AbilityDisplay : MonoBehaviour
{
    [SerializeField] private Image iconImage;
    [SerializeField] private Slider slider;
    private AbilityUpgrade ability;

    private void Update()
    {
        slider.value = ability.RemainingCooldown / ability.baseCooldown;
    }
    public void SetAbility(AbilityUpgrade ability)
    {
        this.ability = ability;
        iconImage.sprite = ability.icon;
    }
}
