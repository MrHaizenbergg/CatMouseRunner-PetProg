using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemGeneratorFabric : Singleton<ItemGeneratorFabric>
{
    [SerializeField] private GameObject[] items;
    [SerializeField] private Animator _anim;
    [SerializeField] private float _forceThrow = 9f;
    [SerializeField] private int _coolDownThrow = 8;
    Coroutine throwItemCoroutine;
    bool pressItemGenerator;
    public float spread;

    public List<GameObject> _inactiveItems = new List<GameObject>();

    private void Awake()
    {
        PoolManager.Instance.Preload(items[0], 1);
        PoolManager.Instance.Preload(items[1], 1);
        PoolManager.Instance.Preload(items[2], 1);
        PoolManager.Instance.Preload(items[3], 1);
        PoolManager.Instance.Preload(items[4], 1);

        //_inactiveItems.Add(items[0]);
        //_inactiveItems.Add(items[1]);
        //_inactiveItems.Add(items[2]);
        //_inactiveItems.Add(items[3]);
        //_inactiveItems.Add(items[4]);
    }

    public void ActiveItems()
    {
        for (int i = 0; i < _inactiveItems.Count; i++)
        {
            _inactiveItems[i].SetActive(true);
        }
    }


    private IEnumerator ThrowItem()
    {
        int RandomNumber = Random.Range(0, items.Length);
        float RandomThrowForce = Random.Range(7f, _forceThrow);
        _anim.SetTrigger("isTurnBack");
        //ActiveItems();
        yield return new WaitForSeconds(1);
        GameObject go = PoolManager.Instance.Spawn((items[RandomNumber]), transform.position, Quaternion.identity);
        //_inactiveItems.Add(go);
        //go.SetActive(true);

        Vector3 pos = new Vector3(0f, 0.9f, -0.6f);

        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);

        float randomX = Random.Range(10f, 100f);
        float randomY = Random.Range(10f, 100f);
        float randomZ = Random.Range(10f, 100f);

        Vector3 dirWithSpread = pos + new Vector3(x, y, 0);

        //rb.AddForce(-pos*13f, ForceMode.VelocityChange);
        go.GetComponentInChildren<Rigidbody>().AddForce(dirWithSpread * RandomThrowForce, ForceMode.Impulse);
        go.GetComponentInChildren<Rigidbody>().AddTorque(randomX, randomY, randomZ);

        Debug.Log("Throw");
        yield return new WaitForSeconds(1.5f);
        //PoolManager.Instance.Despawn(_inactiveItems[0]);
        //_inactiveItems.RemoveAt(0);
        go.SetActive(false);
        yield return new WaitForFixedUpdate();
        Destroy(go);
        yield return new WaitForSeconds(_coolDownThrow);
        if (MouseController.Instance.isJumpingMouse == false)
            throwItemCoroutine = StartCoroutine(ThrowItem());
        else
        {
            yield return new WaitForSeconds(_coolDownThrow);
            if (MouseController.Instance.isJumpingMouse == false)
                throwItemCoroutine = StartCoroutine(ThrowItem());
        }

    }

    private void FixedUpdate()
    {
        if (pressItemGenerator)
            throwItemCoroutine = StartCoroutine(ThrowItem());
        pressItemGenerator = false;
    }
    public void StartThrowItem()
    {
        pressItemGenerator = true;
        //StartCoroutine(ThrowItem());
    }
    public void StopThrowItem()
    {
        //StopCoroutine(throwItemCoroutine);
        StopAllCoroutines();
    }


    //public ItemStats GetItem(ItemType type)
    //{
    //    switch (type)
    //    {
    //        case ItemType.Toaster:
    //            return new ItemStats(strengthKick: 10, speedKick: 10, damageKick: 10);
    //        case ItemType.Cubok1:
    //            return new ItemStats(strengthKick: 10, speedKick: 10,damageKick: 10);
    //        case ItemType.Cubok2:
    //            return new ItemStats(strengthKick: 10, speedKick: 10, damageKick: 10);
    //        case ItemType.Telek:
    //            return new ItemStats(strengthKick: 10, speedKick: 10, damageKick: 10);
    //        case ItemType.Plant:
    //            return new ItemStats(strengthKick: 10, speedKick: 10, damageKick: 10);
    //    }
    //    return null;
    //}

}
