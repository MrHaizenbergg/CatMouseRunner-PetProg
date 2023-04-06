using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dynamit : Singleton<Dynamit>
{
    [SerializeField] private float _forceThrow = 10f;
    [SerializeField] GameObject DynamitObj;
    [SerializeField] GameObject deleteDynInHands;
    [SerializeField] private Transform _spawnDynamit;
    //[SerializeField] private GameObject _mouseTarget;
    [SerializeField] private float radius;
    [SerializeField] private float forceExplosion;
    [SerializeField] private Animator _animCat;
    
    private Coroutine _spawnDynamitCoroutine;
    bool pressThrowDynamit;
    public float spread;

    public void PressThrowDynamit()
    {
        pressThrowDynamit = true;
    }

    private void FixedUpdate()
    {
        if (pressThrowDynamit)
        {
            _spawnDynamitCoroutine = StartCoroutine(ThrowDynamit());
            pressThrowDynamit = false;
        }
    }

    public IEnumerator ThrowDynamit()
    {
        _animCat.SetTrigger("isGrenade");
        deleteDynInHands.SetActive(false);

        Vector3 pos = new Vector3(0f, 0.9f, 0.6f);

        GameObject go = Instantiate(DynamitObj, transform.position, Quaternion.identity);
        go.SetActive(true);

        //Vector3 dirWithoutSpread = _mouseTarget.transform.position - go.transform.position;

        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);

        float randomX = Random.Range(10f, 100f);
        float randomY = Random.Range(10f, 100f);
        float randomZ = Random.Range(10f, 100f);

        Vector3 dirWithSpread = pos + new Vector3(x, y, 0);

        go.GetComponentInChildren<Rigidbody>().AddForce(dirWithSpread * _forceThrow, ForceMode.Impulse);
        go.GetComponentInChildren<Rigidbody>().AddTorque(randomX, randomY, randomZ);
      
        yield return new WaitForSeconds(1.3f);

        Explosion.Instance.ExplosionDyn();
        
        yield return new WaitForSeconds(0.2f);
        go.SetActive(false);
        Destroy(go);
        PlayerController.Instance.LoseDynamit();

    }
}
