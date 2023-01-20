using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileGenerator : MonoBehaviour
{
    public GameObject[] tilePrefabs;

    private float spawnPos = 0;
    private float tileLength = 100;

    void Start()
    {
        CreateNextRoad(0);
        CreateNextRoad(1);
        CreateNextRoad(2);
    }

    
    void Update()
    {
       
    }

    public void CreateNextRoad(int tileIndex)
    {
        Vector3 pos = Vector3.zero;

        //if (roads.Count > 0)
        //{
        //    pos = roads[roads.Count - 1].transform.position + new Vector3(0, 0, 50);

        //}

        GameObject go = Instantiate(tilePrefabs[tileIndex], transform.forward * spawnPos, Quaternion.identity);
        spawnPos += tileIndex;
       // go.transform.SetParent(transform);
        //roads.Add(go);
    }

    




}
