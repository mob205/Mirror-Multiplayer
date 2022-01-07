using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthTracker : MonoBehaviour
{
    public Health target;

    private Slider healthBar;

    private void Start()
    {
        healthBar = GetComponent<Slider>();
    }
    private void Update()
    {
        healthBar.value = target.CurrentHealth / target.MaxHealth;
    }
}
