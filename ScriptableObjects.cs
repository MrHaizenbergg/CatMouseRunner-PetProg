using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor;
using UnityEngine;

public class ScriptableObjects : MonoBehaviour
{
    [SerializeField] private ScriptableObject[] scriptableObjects;
    [SerializeField] private MapDisplayLevSel mapDisplay;


    private int currentIndex;


    private void Awake()
    {
        ChangeScriptableObject(0);
    }

    public void ChangeScriptableObject(int change)
    {
        currentIndex += change;
        if (currentIndex < 0) currentIndex = scriptableObjects.Length - 1;
        else if (currentIndex > scriptableObjects.Length - 1) currentIndex = 0;

        if (mapDisplay != null) mapDisplay.DisplayMap((Map)scriptableObjects[currentIndex]);
    }

}

    //private const string FILENAME = "Map";

    //public void SaveToFile()
    //{
    //    var filePath = Path.Combine(Application.persistentDataPath, FILENAME);

    //    if (!File.Exists(filePath))
    //    {
    //        File.Create(filePath);
    //    }

    //    var json = JsonUtility.ToJson(this);
    //    File.WriteAllText(filePath, json);
    //}


    //public void LoadDataFromFile()
    //{
    //    var filePath = Path.Combine(Application.persistentDataPath, FILENAME);

    //    if (!File.Exists(filePath))
    //    {
    //        Debug.LogWarning($"File \"{filePath}\" not found!", this);
    //        return;
    //    }

    //    var json = File.ReadAllText(filePath);
    //    JsonUtility.FromJsonOverwrite(json, this);
    //}
    //public bool isSavedFile()
    //{
    //    return Directory.Exists(Application.persistentDataPath + "/ScriptableObjects");
    //}
    //public void SavedGame()
    //{
    //    if (!isSavedFile())
    //    {
    //        Directory.CreateDirectory(Application.persistentDataPath + "/ScriptableObjects");
    //    }
    //    if (!Directory.Exists(Application.persistentDataPath + "/ScriptableObjects/Maps"))
    //    {
    //        Directory.CreateDirectory(Application.persistentDataPath + "/ScriptableObjects/Maps");
    //    }
    //    BinaryFormatter bf = new BinaryFormatter();
    //    FileStream file = File.Create(Application.persistentDataPath + "ScriptableObjects/Maps/Map_save");
    //    var json = JsonUtility.ToJson(scriptableObjects);
    //}