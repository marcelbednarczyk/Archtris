using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class MenuManager : MonoBehaviour
{
    public static bool NewGame { get; private set; }

    public void StartButton()
    {
        Destroy(GameObject.Find("Tetris"));
        NewGame = true;
        Time.timeScale = 1;
        SceneManager.LoadScene("Main");
    }

    public void ContinueButton()
    {
        NewGame = false;
        Time.timeScale = 1;
        SceneManager.LoadScene("Main");
    }

    public void QuitButton()
    {
        Application.Quit();
    }
}
