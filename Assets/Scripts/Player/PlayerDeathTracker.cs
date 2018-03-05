// Author(s): Paul Calande
// Rocket Puncher script for counting down the seconds after the player dies.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeathTracker : MonoBehaviour
{
    public class Data
    {
        [Tooltip("How many seconds to wait after the player dies.")]
        public float secondsToWaitAfterDeath;

        public Data(float secondsToWaitAfterDeath)
        {
            this.secondsToWaitAfterDeath = secondsToWaitAfterDeath;
        }
    }
    [SerializeField]
    Data data;
    [SerializeField]
    [Tooltip("Reference to the Score component.")]
    Score score;

    Timer deathWaitTimer;

    public void SetData(Data val)
    {
        data = val;
    }

    private void Start()
    {
        deathWaitTimer = new Timer(data.secondsToWaitAfterDeath, false, false);
    }

    // To be called when the player dies.
    public void PlayerHasDied()
    {
        deathWaitTimer.Start();
        int scoreThisTime = score.GetValue();
        if (scoreThisTime > PlayerPrefs.GetInt("HighScore", 0))
        {
            PlayerPrefs.SetInt("HighScore", scoreThisTime);
        }
    }

    private void FixedUpdate()
    {
        while (deathWaitTimer.TimeUp(Time.deltaTime))
        {
            //UtilScene.ResetScene();
            score.PopulateSummaryScreen();
        }
    }
}