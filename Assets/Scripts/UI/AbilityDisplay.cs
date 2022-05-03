using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AbilityDisplay : MonoBehaviour
{
    [SerializeField] private Image iconImage;
    [SerializeField] private Slider slider;
    [SerializeField] private TextMeshProUGUI chargeText;
    [SerializeField] private GameObject chargeDisplay;
    private AbilityUpgrade ability;

    private void Update()
    {
        if(ability.maxCharges > 1 && ability.RemainingCharges > 0)
        {
            slider.value = ability.RemainingCastDelay / ability.baseCastDelay;
        }
        else
        {
            slider.value = ability.RemainingCooldown / ability.baseCooldown;
        }
        if(chargeText)
        {
            chargeText.text = ability.RemainingCharges.ToString();
        }
    }
    public void SetAbility(AbilityUpgrade ability)
    {
        this.ability = ability;
        iconImage.sprite = ability.icon;
        if(ability.maxCharges == 1)
        {
            chargeDisplay.SetActive(false);
        }
    }
}
