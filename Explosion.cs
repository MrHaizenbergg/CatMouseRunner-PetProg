using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : Singleton<Explosion>
{
    [SerializeField] private float radius;
    [SerializeField] private float forceExplosion;
    [SerializeField] Rigidbody rbMouse;
    [SerializeField] ParticleSystem exploseParticle;


    public void ExplosionDyn()
    {
        Collider[] overlapColliders = Physics.OverlapSphere(transform.position, radius);

        for (int i = 0; i < overlapColliders.Length; i++)
        {
           
            Rigidbody rb = overlapColliders[i].attachedRigidbody;
           
            if (rb != null)
            {
                rb.AddExplosionForce(forceExplosion, transform.position, radius);
                if (rb == rbMouse)
                {
                    HealthMouse.Instance.ChangeHealthMouse(-30);
                }
            }
        }
        exploseParticle.Play();
    }
}
