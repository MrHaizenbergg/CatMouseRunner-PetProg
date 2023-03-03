using UnityEngine.UI;
using UnityEngine;

public class HealthBarMouse : MonoBehaviour
{
    [SerializeField] private Image _healthBarFillMouse;
    [SerializeField] HealthMouse _healthMouse;

    private void OnHealthChangedMouse(float valuePercent)
    {
        _healthBarFillMouse.fillAmount = valuePercent;
    }
    private void OnDestroy()
    {
        _healthMouse.HealtChangeMouse -= OnHealthChangedMouse;
    }

    private void Awake()
    {
        _healthMouse.HealtChangeMouse += OnHealthChangedMouse;
    }

}
