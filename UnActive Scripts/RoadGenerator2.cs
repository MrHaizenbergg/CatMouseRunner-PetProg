using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class RoadGenerator2 : MonoBehaviour
{
    public GameObject[] roads;

    private List<GameObject> ReadyRoad = new List<GameObject>();

    public int currentRoadLength = 0;
    public int maxRoadLength = 3;
    public float distanceBetweenRoads = 10;
    public float speedRoad = 5;
    public float maxPositionZ = -15;

    public Vector3 waitingZona = new Vector3(0, 0, -150);

    public bool[] roadNumbers;

    private int _currentRoadNumber = -1;
    private int _lastRoadNumber = -1;

    public string roadGenerationStatus = "Generation";

    private void FixedUpdate()
    {
        if (roadGenerationStatus == "Generation")
        {
            if (currentRoadLength != maxRoadLength)
            {
                _currentRoadNumber = Random.Range(0, roads.Length);

                if (_currentRoadNumber != _lastRoadNumber)
                {
                    if (_currentRoadNumber < roads.Length / 2)
                    {
                        if (roadNumbers[_currentRoadNumber] != true)
                        {
                            if (_lastRoadNumber != (roads.Length / 2) + _currentRoadNumber)
                            {
                                RoadCreation();
                            }
                            else if (_lastRoadNumber == (roads.Length / 2) + _currentRoadNumber && currentRoadLength == roads.Length - 1)
                            {
                                RoadCreation();
                            }
                        }
                    }
                    else if (_currentRoadNumber >= roads.Length / 2)
                    {
                        if (roadNumbers[_currentRoadNumber] != true)
                        {
                            if (_lastRoadNumber != _currentRoadNumber - (roads.Length / 2))
                            {
                                RoadCreation();
                            }
                            else if (_lastRoadNumber == _currentRoadNumber - (roads.Length / 2) && currentRoadLength == roads.Length - 1)
                            {
                                RoadCreation();
                            }
                        }
                    }
                }
            }
            MovingRoad();

            if(ReadyRoad.Count != 0)
            {
                //RemovingRoad();
            }
        }
    }
    //private void RemovingRoad()
    //{
    //    if (ReadyRoad[0].transform.localPosition.z < maxPositionZ)
    //    {
    //        int i;
    //        i = ReadyRoad[0].GetComponent<Road>().number;
    //        roadNumbers[i] = false;
    //        ReadyRoad[0].transform.localPosition = waitingZona;
    //        ReadyRoad.RemoveAt(0);
    //        currentRoadLength--;
    //    }
    //}
    private void MovingRoad()
    {
        foreach(GameObject readyRoad in ReadyRoad)
        {
            readyRoad.transform.localPosition -= new Vector3(0f, 0f, speedRoad * Time.fixedDeltaTime);
        }
    }
    private void RoadCreation()
    {
        if (ReadyRoad.Count > 0)
        {
            roads[_currentRoadNumber].transform.localPosition = ReadyRoad[ReadyRoad.Count - 1].transform.position + new Vector3(0f, 0f, distanceBetweenRoads);
        }
        else if (ReadyRoad.Count == 0)
        {
            roads[_currentRoadNumber].transform.localPosition = new Vector3(0f, 0f, 0f);
        }

        //roads[_currentRoadNumber].GetComponent<Road>().number = _currentRoadNumber;

        roadNumbers[_currentRoadNumber]=true;

        _lastRoadNumber = _currentRoadNumber;

        ReadyRoad.Add(roads[_currentRoadNumber]);

        currentRoadLength++;
    }
}
