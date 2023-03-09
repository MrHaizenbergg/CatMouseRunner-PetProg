using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthMouse : Singleton<HealthMouse>
{
    [SerializeField] private int _maxHealth = 100;

    private int _currentHealth;
    
    public event Action<float> HealtChangeMouse;

    public void ChangeHealthMouse(int health)
    {
        _currentHealth += health;

        if (_currentHealth !> _maxHealth)
            _currentHealth = health;

        if (_currentHealth <= 0)
        {
            HealtChangeMouse?.Invoke(0);
            _currentHealth = 0;
            PlayerController.Instance.VictoryCat();
            MouseController.Instance.DeathMouse();
            Debug.Log("Mouse is Dead");

        }
        else
        {
            float _currentHealthInPercent = (float)_currentHealth / _maxHealth;
            HealtChangeMouse?.Invoke(_currentHealthInPercent);
        }

    }
    private void Start()
    {
        _currentHealth = _maxHealth;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            ChangeHealthMouse(-10);
        }
    }
}
