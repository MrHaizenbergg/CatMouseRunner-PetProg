using UnityEngine.UI;
using UnityEngine;

public class HealthBarMouse : MonoBehaviour
{
    [SerializeField] private Image healthBarFillMouse;
    [SerializeField] HealthMouse healthMouse;

    private void OnHealthChangedMouse(float valuePercent)
    {
        healthBarFillMouse.fillAmount = valuePercent;
    }
    private void OnDestroy()
    {
        healthMouse.HealtChangeMouse -= OnHealthChangedMouse;
    }

    private void Awake()
    {
        healthMouse.HealtChangeMouse += OnHealthChangedMouse;
    }

}
