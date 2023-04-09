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