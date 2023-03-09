using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : Singleton<Health>
{
    [SerializeField] private int _maxHealth = 100;
    private int _currentHealth;

    public event Action<float> HealtChange;

    public void ChangeHealth(int health)
    {
        _currentHealth += health;

        if (_currentHealth! > _maxHealth)
            _currentHealth = health;
        

        if (_currentHealth <= 0)
        {
            HealtChange?.Invoke(0);
            _currentHealth = 0;
            PlayerController.Instance.Death();
            Debug.Log("Cat is Dead");

        }
        else
        {
            float _currentHealthInPercent = (float)_currentHealth / _maxHealth;
            HealtChange?.Invoke(_currentHealthInPercent);
        }

    }
    private void Start()
    {
        _currentHealth = _maxHealth;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            ChangeHealth(-10);
        }
    }
}
