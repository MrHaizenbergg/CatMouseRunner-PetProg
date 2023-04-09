using UnityEngine;

public class GreatWeapon : MonoBehaviour
{
    [SerializeField] private float _kickForce = 10f;
    [SerializeField] private float _coolDownAttack = 5f;

    private bool _activeAttack;
    private bool _takeDamage;
    private float _nextAttackTime = 0f;

    private void FixedUpdate()
    {
        if (_activeAttack)
        {
            if (Time.time > _nextAttackTime)
            {
                AttackGreatWeapon();
                _nextAttackTime = Time.time + _coolDownAttack;
            }
        }
        _activeAttack = false;
    }

    public void PressAttackGreatWeapon()
    {
        _activeAttack = true;
    }

    public void AttackGreatWeapon()
    {
        PlayerController.Instance.anim.SetTrigger("isAttackGreatWeapon");
        _takeDamage = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Mouse")
        {
            if (_takeDamage)
            {
                HealthMouse.Instance.ChangeHealthMouse(-40);
            }
            _takeDamage = false;
        }
    }
}


