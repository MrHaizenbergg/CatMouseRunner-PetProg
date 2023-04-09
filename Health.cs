using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : Singleton<Health>
{
    [SerializeField] private int _maxHealth = 100;
    private bool isImmortalCat;
    private int _currentHealth;

    public event Action<float> HealthChange;

    public void ImmortalCatTrue()
    {
        isImmortalCat = true;
    }

    public void ImmortalCatFalse()
    {
        isImmortalCat = false;
    }

    public void ChangeHealth(int health)
    {
        if (isImmortalCat)
        {
            HealthChange?.Invoke(_currentHealth);
        }
        else
        {
            _currentHealth += health;

            if (_currentHealth! > _maxHealth)
                _currentHealth = health;

            if (_currentHealth <= 0)
            {
                HealthChange?.Invoke(0);
                _currentHealth = 0;
                PlayerController.Instance.Death();
                Debug.Log("Cat is Dead");

            }
            else
            {
                float _currentHealthInPercent = (float)_currentHealth / _maxHealth;
                HealthChange?.Invoke(_currentHealthInPercent);
            }
        }
    }
    private void Start()
    {
        _currentHealth = _maxHealth;
    }
}
