using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using UnityEngine;


public class RoadGenerator : Singleton<RoadGenerator>
{
    [SerializeField] private GameObject[] Roads;
    [SerializeField] private GameObject[] turnRoads;
    [SerializeField] private float Maxspeed = 15;
    [SerializeField] private int MaxRoadCount = 4;

    Vector3 turn;
    bool isTurnLeft = false;
    bool isTurnRight = false;

    private List<GameObject> roads = new List<GameObject>();
    private List<GameObject> roadsLeft = new List<GameObject>();

    public float currentspeed = 0;
    Coroutine speedCoroutine;
    //delegate void SpeedIncrease();
    //static SpeedIncrease speedIncrease = StartCoroutine();

    private void Awake()
    {
        PoolManager.Instance.Preload(Roads[0], 3);
        PoolManager.Instance.Preload(Roads[1], 3);
        PoolManager.Instance.Preload(turnRoads[0], 3);
        //PoolManager.Instance.Preload(Roads[2], 3);
        //PoolManager.Instance.Preload(Roads[3], 3);
    }

    void Start()
    {
        //StartCoroutine(SpeedIncrease());
        ResetLevel();
    }

    public void TurnLeft()
    {
        isTurnLeft = true;
        PlayerController.Instance.CatTurnLeft();
    }
    public void TurnRight()
    {
        isTurnRight = true;
        PlayerController.Instance.CatTurnRight();
    }

    void Update()
    {
        if (currentspeed == 0) return;

        foreach (GameObject Road in roads)
        {
            if (Road != null)
            {
                if (isTurnLeft)
                {
                    //TurnLeftCat();
                    if (roads[0].transform.position.x < 150.5f)
                    {

                        PoolManager.Instance.Despawn(roads[0]);
                        roads.RemoveAt(0);

                        CreateNextRoad(Random.Range(0, Roads.Length - 2));
                    }

                    Road.transform.position -= new Vector3(currentspeed * Time.deltaTime, 0, 0);

                }

                if (isTurnRight)
                {
                    //TurnRightCat();
                    if (roads[0].transform.position.x < -150.5f)
                    {


                        PoolManager.Instance.Despawn(roads[0]);
                        roads.RemoveAt(0);

                        CreateNextRoad(Random.Range(0, Roads.Length - 2));
                    }

                    Road.transform.position -= new Vector3(currentspeed * Time.deltaTime, 0, 0);

                }

                if (isTurnLeft == false && isTurnRight == false)
                {
                    if (roads[0].transform.position.z < -150.5f)
                    {


                        PoolManager.Instance.Despawn(roads[0]);
                        roads.RemoveAt(0);

                        CreateNextRoad(Random.Range(0, Roads.Length - 2));

                        int i = Random.Range(1, 4);
                        //switch (i)
                        //{
                        //    //case 1:
                        //    //    CreateNextRoad(Random.Range(0, Roads.Length-1));
                        //    //    break;
                        //    case 2:
                        //        //TurnLeft();
                        //        isTurnLeft = true;
                        //        //CreateNextRoad(Random.Range(0, Roads.Length - 2));
                        //        //Debug.Log("TurnLeft");
                        //        //CreateNextRoadLeft(Random.Range(0, Roads.Length-1));
                        //        break;
                        //    case 3:
                        //        //TurnRight();
                        //        isTurnRight = true;
                        //        //CreateNextRoad(Random.Range(0, Roads.Length - 2));
                        //        //Debug.Log("TurnRight");
                        //        break;
                        //    default:
                        //        CreateNextRoad(Random.Range(0, Roads.Length - 2));
                        //        break;
                        //}


                    }

                    Road.transform.position -= new Vector3(0, 0, currentspeed * Time.deltaTime);
                }
            }


            //if (Road != null)
            //    Road.transform.position -= new Vector3(0, 0, currentspeed * Time.deltaTime);
        }




    }

    public void StopGame()
    {
        currentspeed = 0;
    }

    void CreateNextRoad(int index)
    {
        Vector3 pos = Vector3.zero;
        Quaternion quaternion = Quaternion.identity;

        if (roads.Count > 0)
        {
            if (isTurnLeft)
            {
                pos = roads[roads.Count - 1].transform.position + new Vector3(-146f, 0, 0);
                quaternion = turnRoads[0].transform.rotation = Quaternion.Euler(0, 90, 0);
                Debug.Log("TurnLeftIs");
                //isTurnLeft=false;
            }
            if (isTurnRight)
            {
                pos = roads[roads.Count - 1].transform.position + new Vector3(146f, 0, 0);
                quaternion = turnRoads[0].transform.rotation = Quaternion.Euler(0, -90, 0);
                Debug.Log("TurnRightIs");
                //isTurnLeft=false;
            }
            if (isTurnLeft == false && isTurnRight == false)
            {
                pos = roads[roads.Count - 1].transform.position + new Vector3(0, 0, 146.5f);
                quaternion = turnRoads[0].transform.rotation = Quaternion.identity;
                Debug.Log("UsualNextRoad");
            }
            isTurnLeft = false;
            isTurnRight = false;
        }

        GameObject go = PoolManager.Instance.Spawn(Roads[index], pos, quaternion);
        go.transform.SetParent(transform, true);
        roads.Add(go);

    }

    //void CreateNextRoadLeft(int index)
    //{
    //    Vector3 pos = Vector3.zero;

    //        pos = roads[roads.Count].transform.position + new Vector3(0, 0, -140f);

    //    GameObject go = PoolManager.Instance.Spawn(Roads[index], pos, Quaternion.identity);
    //    go.transform.SetParent(transform, true);
    //    roads.Add(go);
    //    Debug.Log("NextRoadLeft");
    //}

    void TurnLeftCat()
    {
        Vector3 pos = Vector3.zero;

        pos = turnRoads[0].transform.position + new Vector3(0, 0, 135f);

        //Quaternion quaternion = turnRoads[0].transform.rotation = Quaternion.identity;

        GameObject go = PoolManager.Instance.Spawn(Roads[3], pos, Quaternion.identity);
        go.transform.SetParent(transform, true);
        roadsLeft.Add(go);

    }

    void TurnRightCat()
    {
        Vector3 pos = Vector3.zero;

        pos = turnRoads[1].transform.position + new Vector3(0, 0, 135f);

        //Quaternion quaternion = turnRoads[1].transform.rotation = Quaternion.identity;

        GameObject go = PoolManager.Instance.Spawn(Roads[4], pos, Quaternion.identity);
        go.transform.SetParent(transform.transform, true);
        roadsLeft.Add(go);

    }

        public void StartLevel()
        {
            currentspeed = Maxspeed;
            SwipeManager.instance.enabled = true;
        }


        public void ResetLevel()
        {
            currentspeed = 0;

            isTurnLeft = false;
            isTurnRight = false;

            while (roads.Count > 0)
            {
                Destroy(roads[0]);
                roads.RemoveAt(0);
            }

            for (int i = 0; i < MaxRoadCount; i++)
            {
                //if (roads.Count % 2 == 0)
                //{
                //    CreateNextRoad(Random.Range(0, Roads.Length));
                //}
                //else
                //{
                //    TurnLeft();
                //    CreateNextRoadLeft(Random.Range(0, turnRoads.Length));
                //}

                CreateNextRoad(Random.Range(0, Roads.Length - 3));

            }
            SwipeManager.instance.enabled = false;
            MapGenerator.Instance.ResetMaps();

        }
    }
