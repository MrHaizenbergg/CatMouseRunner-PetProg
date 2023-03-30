using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using UnityEngine;


public class RoadGenerator : Singleton<RoadGenerator>
{
    [SerializeField] private GameObject[] Roads;
    [SerializeField] private float Maxspeed = 15;
    [SerializeField] private int MaxRoadCount = 4;

    private List<GameObject> roads = new List<GameObject>();

    public float currentspeed = 0;
    //delegate void SpeedIncrease();
    //static SpeedIncrease speedIncrease = StartCoroutine();

    private void Awake()
    {
        PoolManager.Instance.Preload(Roads[0], 3);
        PoolManager.Instance.Preload(Roads[1], 3);
        //PoolManager.Instance.Preload(Roads[2], 3);
        //PoolManager.Instance.Preload(Roads[3], 3);
    }

    void Start()
    {
        //StartCoroutine(SpeedIncrease());
        ResetLevel();
    }

    void Update()
    {
        if (currentspeed == 0) return;

        foreach (GameObject Road in roads)
        {
            if (Road != null)
                Road.transform.position -= new Vector3(0, 0, currentspeed * Time.deltaTime);
        }

        if (roads[0].transform.position.z < -150.5f)
        {
            PoolManager.Instance.Despawn(roads[0]);
            roads.RemoveAt(0);

            CreateNextRoad(Random.Range(0, Roads.Length));
        }
    }

    public void StopGame()
    {
        currentspeed = 0;
    }

    void CreateNextRoad(int index)
    {
        Vector3 pos = Vector3.zero;

        if (roads.Count > 0)
        {
            pos = roads[roads.Count - 1].transform.position + new Vector3(0, 0, 146.5f);
        }

        GameObject go = PoolManager.Instance.Spawn(Roads[index], pos, Quaternion.identity);
        go.transform.SetParent(transform, true);
        roads.Add(go);
    }


    public void StartLevel()
    {
        currentspeed = Maxspeed;
        SwipeManager.instance.enabled = true;
    }


    public void ResetLevel()
    {
        currentspeed = 0;

        while (roads.Count > 0)
        {
            Destroy(roads[0]);

            roads.RemoveAt(0);
        }

        for (int i = 0; i < MaxRoadCount; i++)
        {
            CreateNextRoad(Random.Range(0, Roads.Length));
        }
        SwipeManager.instance.enabled = false;
        MapGenerator.Instance.ResetMaps();
    }


}
