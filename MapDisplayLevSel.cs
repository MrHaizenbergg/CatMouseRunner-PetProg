using UnityEngine.UI;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Events;


public class MapDisplayLevSel : MonoBehaviour
{
    [SerializeField] private TMP_Text mapName;
    [SerializeField] private TMP_Text mapDescription;
    [SerializeField] private Image mapImage;
    [SerializeField] private Button playButton;
    [SerializeField] private GameObject lockImage;
    public UnityAction OnClick;

    public void DisplayMap(Map map)
    {
        mapName.text = map.name;
        mapDescription.text = map.mapDescription;
        mapImage.sprite = map.mapImage;

        bool mapUnlocked = PlayerPrefs.GetInt("currentScene", 0) >= map.mapIndex;

        lockImage.SetActive(!mapUnlocked);
        playButton.interactable=mapUnlocked;
        
        if (mapUnlocked)
            mapImage.color=Color.white;
        else
            mapImage.color=Color.grey;

         
        playButton?.onClick.RemoveAllListeners();
        playButton?.onClick.AddListener(() => SceneManager.LoadScene(map.SceneToLoad.name));

    }

}
