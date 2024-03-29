using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthMouse :Singleton<HealthMouse>
{
    [SerializeField] private int maxHealth = 100;

    private MouseController _mouseController;

    public HealthMouse(MouseController mouseController)
    {
        _mouseController=mouseController;
    }
    private int _currentHealth;

    public event Action<float> HealtChangeMouse;

    public void ChangeHealthMouse(int health)
    {
        _currentHealth += health;

        if (_currentHealth !> maxHealth)
            _currentHealth = health;

        if (_currentHealth <= 0)
        {
            HealtChangeMouse?.Invoke(0);
            _currentHealth = 0;
            PlayerController.Instance.VictoryCat();
            //MouseController.DeathMouse();
            _mouseController.DeathMouse();
            Debug.Log("Mouse is Dead");

        }
        else
        {
            float _currentHealthInPercent = (float)_currentHealth / maxHealth;
            HealtChangeMouse?.Invoke(_currentHealthInPercent);
        }

    }
    private void Start()
    {
        _mouseController=GetComponent<MouseController>();
        _currentHealth = maxHealth;
    }
}
