using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneController
{
    public static int deathCount = 0;
    public static void LoadLevelOne()
    {
        SceneManager.LoadScene("LevelOne");
        deathCount++;
    }

    public static void LoadWinScreen()
    {
        deathCount = 0;
        SceneManager.LoadScene("WinScreen");
    }

    public static void LoadStartScreen()
    {
        deathCount = 0;
        SceneManager.LoadScene("StartScreen");
    }
}
