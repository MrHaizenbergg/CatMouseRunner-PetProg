using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;
using UnityEngine.UI;

public class MainMenu : Singleton<MainMenu>
{
    [SerializeField] private Text coinsText;

    private void Start()
    {
        int coins = PlayerPrefs.GetInt("coins");
        coinsText.text = coins.ToString();
    }
    public void PlayGame()
    {
        SceneManager.LoadScene(0);
        //PlayerController.Instance.StartGame();
    }
    
}
