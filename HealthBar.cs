using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image _healthBarFill;
    [SerializeField] Health _health;

    private void OnHealthChanged(float valuePercent)
    {
        _healthBarFill.fillAmount = valuePercent;
    }
    private void OnDestroy()
    {
        _health.HealtChange -= OnHealthChanged;
    }

    private void Awake()
    {
        _health.HealtChange += OnHealthChanged;
    }

}
