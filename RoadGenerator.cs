using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using static UnityEngine.UI.Image;

public class RoadGenerator : Singleton<RoadGenerator>
{
    [SerializeField] private GameObject[] Roads;
    [SerializeField] private float Maxspeed = 15;
    [SerializeField] private int MaxRoadCount = 2;

    private List<GameObject> roads = new List<GameObject>();
    public float currentspeed = 0;
    //delegate void SpeedIncrease();
    //static SpeedIncrease speedIncrease = StartCoroutine();

    private void Awake()
    {
        
        roads.Add(Roads[0]);
        roads.Add(Roads[1]);
        //PoolManager.Instance.Preload(Roads[0], 15);
        //PoolManager.Instance.Preload(Roads[1], 15);
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

        if (roads[0].transform.position.z < -148.5f)
        {
            roads[0].SetActive(false);
            //PoolManager.Instance.Despawn(roads[0]);
            Destroy(roads[0]);
            roads.RemoveAt(0);

            CreateNextRoad(0);
        }
    }

    void CreateNextRoad(int index)
    {
        Vector3 pos = Vector3.zero;


        if (roads.Count > 2)
        {
            pos = roads[roads.Count - 1].transform.position + new Vector3(0, 0, 146.5f);
        }

        GameObject go = PoolManager.Instance.Spawn(roads[index], pos, Quaternion.identity);
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
        GameObject road = Roads[0];
        roads.Add(road);
        GameObject road2 = Roads[1];
        roads.Add(road2);
        //roads.Add(Roads[0]);
        //roads.Add(Roads[1]);
        //PoolManager.Instance.Preload(Roads[0], 15);
        //PoolManager.Instance.Preload(Roads[1], 15);

        while (roads.Count > 2)
        {
            if (Roads != null)
            {
                roads[0].SetActive(false);
                //PoolManager.Instance.Despawn(roads[0]);
                Destroy(roads[0]);
            }
            roads.RemoveAt(0);
        }

        for (int i = 0; i < MaxRoadCount; i++)
        {
            CreateNextRoad(Random.Range(0, roads.Count - 1));
        }
        SwipeManager.instance.enabled = false;
        MapGenerator.Instance.ResetMaps();
    }

    public IEnumerator SpeedIncrease()
    {
        currentspeed += 5;
        yield return new WaitForSeconds(4);
        if (currentspeed > Maxspeed)
        {
            currentspeed -= 5;
            //StartCoroutine(SpeedIncrease());
        }
    }
}