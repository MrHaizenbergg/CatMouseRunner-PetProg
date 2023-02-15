using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemGeneratorFabric : Singleton<ItemGeneratorFabric>
{
    [SerializeField] private GameObject[] items;
    [SerializeField] private Transform _pointToPlayer;
    [SerializeField] private Animator _anim;

    private Coroutine _coroutine;

    public IEnumerator ThrowItem()
    {
        int RandomNumber = Random.Range(0, items.Length);
        _anim.SetTrigger("isTurnBack");
        yield return new WaitForSeconds(1);
        GameObject go = Instantiate((items[RandomNumber]),transform.position,Quaternion.identity);

        Rigidbody rb = go.GetComponentInChildren<Rigidbody>();
        //rb.AddForce(-transform.position*4f, ForceMode.Impulse);
        rb.velocity = new Vector3(0f, 0f, -15f);
        //rb.transform.position= Vector3.MoveTowards(_pointToPlayer.transform.position, transform.position, 50f * Time.deltaTime);
        Debug.Log("Throw");
        yield return new WaitForSeconds(1.5f);
        Destroy(go);
        yield return new WaitForSeconds(8);
        _coroutine = StartCoroutine(ItemGeneratorFabric.Instance.ThrowItem());

    }

    public void StopThrowItem()
    {
        StopCoroutine(_coroutine);
    }

    public ItemStats GetItem(ItemType type)
    {
        switch (type)
        {
            case ItemType.Toaster:
                return new ItemStats(strengthKick: 10, speedKick: 10, damageKick: 10);
            case ItemType.Cubok1:
                return new ItemStats(strengthKick: 10, speedKick: 10,damageKick: 10);
            case ItemType.Cubok2:
                return new ItemStats(strengthKick: 10, speedKick: 10, damageKick: 10);
            case ItemType.Telek:
                return new ItemStats(strengthKick: 10, speedKick: 10, damageKick: 10);
            case ItemType.Plant:
                return new ItemStats(strengthKick: 10, speedKick: 10, damageKick: 10);
        }
        return null;
    }

}
