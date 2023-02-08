using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEditor.Progress;

public class ItemGeneratorFabric : Singleton<ItemGeneratorFabric>
{
    [SerializeField] private GameObject[] items;
    [SerializeField] private Transform _pointToPlayer;
    [SerializeField] private Animator _anim;
    [SerializeField] private float AngleInDegreece;

    public void Start()
    {
        
    }
    public IEnumerator ThrowItem()
    {
        Vector3 direction;
        int RandomNumber = Random.Range(0, items.Length);
        //bool isFlyItem;
        //float x=fromToXZ.magnitude;
        //float y=fromTo.y;
        _anim.SetTrigger("isTurnBack");
        yield return new WaitForSeconds(1);
        GameObject go = Instantiate((items[RandomNumber]),transform.position,Quaternion.identity);

        Rigidbody rb = go.GetComponentInChildren<Rigidbody>();
        //rb.AddForce(-transform.position*4f, ForceMode.Impulse);
        rb.velocity = new Vector3(0f, 0f, -10f);
        //rb.transform.position= Vector3.MoveTowards(_pointToPlayer.transform.position, transform.position, 50f * Time.deltaTime);
        Debug.Log("Throw");
        yield return new WaitForSeconds(1.5f);
        Destroy(go);
        yield return new WaitForSeconds(8);
        StartCoroutine(ThrowItem());

    }

    //public ItemStats GetItem(ItemType type)
    //{
    //    switch (type)
    //    {
    //        case ItemType.Toaster:
    //            //return new ItemStats(strengthKick: 10, speedKick: 10, damageKick: 10);
    //        case ItemType.Cubok1:
    //            ////return new ItemStats(strengthKick: 10, speedKick: 10,damageKick: 10);
    //        case ItemType.Cubok2:
    //            //return new ItemStats(strengthKick: 10, speedKick: 10, damageKick: 10);
    //        case ItemType.Telek:
    //            //return new ItemStats(strengthKick: 10, speedKick: 10, damageKick: 10);
    //        case ItemType.Plant:
    //            //return new ItemStats(strengthKick: 10, speedKick: 10, damageKick: 10);
    //    }
    //    return null;
    //}

}
