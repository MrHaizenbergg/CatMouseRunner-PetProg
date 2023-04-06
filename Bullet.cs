using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private int _damage = 10;
    private Coroutine _liveBullet;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Mouse")
        {
            HealthMouse mouse = collision.transform.GetComponent<HealthMouse>();
            
            if (mouse != null)
            {
                Animator anim = mouse.GetComponent<Animator>();
                anim.SetTrigger("isHitMouse");
                mouse.ChangeHealthMouse(-_damage);
                Destroy(gameObject);
            }
        }
        else
            _liveBullet=StartCoroutine(TimeToLiveBullet());
    }
    IEnumerator TimeToLiveBullet()
    {
        yield return new WaitForSeconds(3);
        Destroy(gameObject);
        StopCoroutine(_liveBullet);
    }
}