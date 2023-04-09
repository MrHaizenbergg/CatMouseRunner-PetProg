using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Map", menuName = "ScriptableObjects/Map")]
public class Map : ScriptableObject
{
    public int mapIndex;
    public string mapName;
    public string mapDescription;
    public Sprite mapImage;
    public Object SceneToLoad;
}

