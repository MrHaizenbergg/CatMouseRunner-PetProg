using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemGeneratorFabric : Singleton<ItemGeneratorFabric>
{
    [SerializeField] private GameObject[] items;
    [SerializeField] private Animator anim;
    [SerializeField] private float forceThrow;
    [SerializeField] private int coolDownThrow = 8;
    [SerializeField] private float spread;

    Coroutine throwItemCoroutine;

    bool pressItemGenerator;

    public List<GameObject> _inactiveItems = new List<GameObject>();

    private void Awake()
    {
        PoolManager.Instance.Preload(items[0], 1);
        PoolManager.Instance.Preload(items[1], 1);
        PoolManager.Instance.Preload(items[2], 1);
        PoolManager.Instance.Preload(items[3], 1);
        PoolManager.Instance.Preload(items[4], 1);

    }

    //public void ActiveItems()
    //{
    //    for (int i = 0; i < _inactiveItems.Count; i++)
    //    {
    //        _inactiveItems[i].SetActive(false);
    //    }
    //}

    private IEnumerator ThrowItem()
    {
        int RandomNumber = Random.Range(0, items.Length);
        float RandomThrowForce = Random.Range(6f, forceThrow);
        anim.SetTrigger("isTurnBack");
    
        yield return new WaitForSeconds(1);
        GameObject go = PoolManager.Instance.Spawn((items[RandomNumber]), transform.position, Quaternion.identity);
        
        Vector3 pos = new Vector3(0f, 0.5f, -1f);

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
        //go.SetActive(false);
        _inactiveItems.Add(go);
        yield return new WaitForFixedUpdate();
        Destroy(go);
        yield return new WaitForSeconds(coolDownThrow);
        if (MouseController.Instance.isJumpingMouse == false)
            throwItemCoroutine = StartCoroutine(ThrowItem());
        else
        {
            yield return new WaitForSeconds(coolDownThrow);
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
    }
    public void StopThrowItem()
    {
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
