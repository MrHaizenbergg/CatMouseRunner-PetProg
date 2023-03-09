using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemGeneratorFabric : Singleton<ItemGeneratorFabric>
{
    [SerializeField] private GameObject[] items;
    [SerializeField] private Transform _pointToPlayer;
    [SerializeField] private Animator _anim;
    Coroutine throwItemCoroutine;

    public List<GameObject> _inactiveItems = new List<GameObject>();

    private void Awake()
    {
        _inactiveItems.Add(items[0]);
        _inactiveItems.Add(items[1]);
        _inactiveItems.Add(items[2]);
        _inactiveItems.Add(items[3]);
        _inactiveItems.Add(items[4]);
    }

    private IEnumerator ThrowItem()
    {
        int RandomNumber = Random.Range(0, _inactiveItems.Count);
        _anim.SetTrigger("isTurnBack");
        yield return new WaitForSeconds(1);
        GameObject go = Instantiate((_inactiveItems[RandomNumber]), transform.position, Quaternion.identity);
        go.SetActive(true);
        Rigidbody rb = go.GetComponentInChildren<Rigidbody>();
        Vector3 pos = new Vector3(0f,0f,1f);
        rb.AddForce(-pos*13f, ForceMode.VelocityChange);
        Debug.Log("Throw");
        yield return new WaitForSeconds(1.5f);
        go.SetActive(false);
        yield return new WaitForFixedUpdate();
        Destroy(go);
 
        yield return new WaitForSeconds(8);
        if (MouseController.Instance.isJumpingMouse == false)
            throwItemCoroutine = StartCoroutine(ThrowItem());

    }

    public void StartThrowItem()
    {
        StartCoroutine(ThrowItem());
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
