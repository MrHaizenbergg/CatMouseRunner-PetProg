using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName ="New Map",menuName ="Scriptable Objects/Map")]
public class Map : ScriptableObject
{
    public int mapIndex;
    public string mapName;
    public string mapDescription;
    public Sprite mapImage;
    public Object SceneToLoad;
}
