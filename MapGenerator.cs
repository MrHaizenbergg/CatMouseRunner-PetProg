using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : Singleton<MapGenerator>
{
    [SerializeField] private int itemSpace = 15;
    [SerializeField] private int itemCountInMap = 5;
    [SerializeField] private int coinsCountInItem = 10;
    [SerializeField] private float coinsHeith = 0.5f;

    public float laneOffset=2.5f;
    int mapSize;

    enum TrackPos { Left = -1, Center = 0, Right = 1 }
    enum CoinStyle { Line, Jump, Ramp }

    public GameObject ObstacleBottomPrefab;
    public GameObject ObstaclFullPrefab;
    public GameObject ObstacleTopPrefab;
    public GameObject RampPrefab;
    public GameObject CoinPrefab;

    public List<GameObject> maps=new List<GameObject>();
    public List<GameObject> activemaps = new List<GameObject>();


    struct MapItem
    {
        public void SetValues(GameObject obstacle,TrackPos trackPos,CoinStyle coinStyle)
        { this.obstacle = obstacle; this.trackPos = trackPos; this.coinStyle = coinStyle; }
        public GameObject obstacle;
        public TrackPos trackPos;
        public CoinStyle coinStyle;
    }

    private void Awake()
    {
        mapSize = itemCountInMap * itemSpace;
        maps.Add(MakeMap1());
        maps.Add(MakeMap1());
        maps.Add(MakeMap1());
        foreach (GameObject map in maps)
        {
            map.SetActive(false);
        }
    }

    void Update()
    {
        if (RoadGenerator.Instance.currentspeed == 0) return;

        foreach (GameObject map in activemaps)
        {
            map.transform.position -= new Vector3(0, 0, RoadGenerator.Instance.currentspeed * Time.deltaTime);
        }
        if (activemaps[0].transform.position.z < -mapSize)
        {
            RemoveFirstActiveMap();
            AddActiveMap();
        }
    }

    void RemoveFirstActiveMap()
    {
        activemaps[0].SetActive(false);
        maps.Add(activemaps[0]);
        activemaps.RemoveAt(0);
    }

    public void ResetMaps()
    {
        while(activemaps.Count > 0)
        {
            RemoveFirstActiveMap();
        }
        AddActiveMap();
        AddActiveMap();
    }

    void AddActiveMap()
    {
        int r = Random.Range(0, maps.Count);
        GameObject go = maps[r];
        go.SetActive(true);
        foreach(Transform child in go.transform)
        {
            child.gameObject.SetActive(true);
        }
        go.transform.position = activemaps.Count > 0 ?
                                activemaps[activemaps.Count - 1].transform.position + Vector3.forward * mapSize :
                                new Vector3(0, 0, 10);
        maps.RemoveAt(r);
        activemaps.Add(go);
    }

    GameObject MakeMap1()
    {
        GameObject result = new GameObject("Map1");
        result.transform.SetParent(transform);
        MapItem item = new MapItem();
        for (int i = 0; i < itemCountInMap; i++)
        {
            item.SetValues(null,TrackPos.Center,CoinStyle.Line);
            
            if (i == 2) { item.SetValues(RampPrefab, TrackPos.Left, CoinStyle.Ramp); }
            else if ( i == 3 ) { item.SetValues(ObstacleBottomPrefab,TrackPos.Right,CoinStyle.Jump); }
            else if ( i == 4 ) { item.SetValues(ObstacleBottomPrefab, TrackPos.Right, CoinStyle.Jump); }

            Vector3 obstaclePos = new Vector3((int)item.trackPos*laneOffset, 0, i * itemSpace);
            CreateCoins(item.coinStyle, obstaclePos, result);


            if (item.obstacle != null)
            {
                GameObject go = Instantiate(item.obstacle, obstaclePos, Quaternion.identity);
                go.transform.SetParent(result.transform);
            }
        }
        return result;
    }

    GameObject MakeMapLeft()
    {
        GameObject result = new GameObject("MapLeft");
        Quaternion quaternion =result.transform.rotation = Quaternion.Euler(0,90,0);
        result.transform.rotation.Equals(quaternion);
        result.transform.SetParent(activemaps[activemaps.Count - 1].transform);

        MapItem item = new MapItem();
        for (int i = 0; i < itemCountInMap; i++)
        {
            item.SetValues(null, TrackPos.Center, CoinStyle.Line);

            if (i == 2) { item.SetValues(RampPrefab, TrackPos.Left, CoinStyle.Ramp); }
            else if (i == 3) { item.SetValues(ObstacleBottomPrefab, TrackPos.Right, CoinStyle.Jump); }
            else if (i == 4) { item.SetValues(ObstacleBottomPrefab, TrackPos.Right, CoinStyle.Jump); }

            Vector3 obstaclePos = new Vector3((int)item.trackPos * laneOffset, 0, i * itemSpace);
            
            CreateCoins(item.coinStyle, obstaclePos, result);


            if (item.obstacle != null)
            {
                GameObject go = Instantiate(item.obstacle, obstaclePos, Quaternion.identity);
                go.transform.SetParent(result.transform);
            }
        }
        return result;
    }

    void CreateCoins(CoinStyle style, Vector3 pos,GameObject parentObject)
    {
        Vector3 coinPos = Vector3.zero;
        if (style == CoinStyle.Line)
        {
            for(int i = -coinsCountInItem/2; i < coinsCountInItem/2; i++)
            {
                coinPos.y = coinsHeith;
                coinPos.z = i*((float)itemSpace/coinsCountInItem);
                GameObject go = Instantiate(CoinPrefab, coinPos + pos, Quaternion.identity);
                go.transform.SetParent(parentObject.transform);
            }
        }
        else if(style == CoinStyle.Jump)
        {
            for (int i = -coinsCountInItem / 2; i < coinsCountInItem / 2; i++)
            {
                coinPos.y =Mathf.Max( -1 / 2f * Mathf.Pow(i, 2) + 3,coinsHeith);
                coinPos.z = i * ((float)itemSpace / coinsCountInItem);
                GameObject go = Instantiate(CoinPrefab, coinPos + pos, Quaternion.identity);
                go.transform.SetParent(parentObject.transform);
            }
        }
        else if (style == CoinStyle.Ramp)
        {
            for (int i = -coinsCountInItem / 2; i < coinsCountInItem / 2; i++)
            {
                coinPos.y = Mathf.Min(Mathf.Max(0.5f *(i+2),coinsHeith),2f);
                coinPos.z = i * ((float)itemSpace / coinsCountInItem);
                GameObject go = Instantiate(CoinPrefab, coinPos + pos, Quaternion.identity);
                go.transform.SetParent(parentObject.transform);
            }
        }
    }
}
