// Author(s): Paul Calande
// Rocket Puncher summary screen.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SummaryScreen : MonoBehaviour
{
    public void PlayAgain()
    {
        UtilScene.ResetScene();
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}