using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : Singleton<MainMenu>
{
    public void PlayGame()
    {
        SceneManager.LoadScene(2);
    }
    public void StopGame()
    {
        SceneManager.LoadScene(0);
        PlayerController.Instance.ResetGame();
    }
    public void SelectLevel()
    {
        SceneManager.LoadScene(1);
    }
    public void ExitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
