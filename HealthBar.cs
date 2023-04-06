using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image healthBarFill;
    [SerializeField] private Health health;

    private void OnHealthChanged(float valuePercent)
    {
        healthBarFill.fillAmount = valuePercent;
    }
    private void OnDestroy()
    {
        health.HealthChange -= OnHealthChanged;
    }

    private void Awake()
    {
        health.HealthChange += OnHealthChanged;
    }

}
