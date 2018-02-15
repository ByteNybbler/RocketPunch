// Author(s): Paul Calande
// Score script for Rocket Puncher.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

public class Score : MonoBehaviour
{
    [SerializeField]
    [Tooltip("File to read score tuning from.")]
    TextAsset scoreFile;
    [SerializeField]
    [Tooltip("Text to use for the score.")]
    UIValueText textScore;

    int pointsPerSecondPlaying;
    int pointsPerEnemyKilled;
    int pointsPerProjectilePunched;
    int pointsPerFullHealthHealthKit;
    Timer timerPointsPerSecond = new Timer(1.0f);

    private void Awake()
    {
        Tune();
    }

    private void Tune()
    {
        JSONNode json = JSON.Parse(scoreFile.ToString());
        pointsPerSecondPlaying = json["points per second playing"].AsInt;
        pointsPerEnemyKilled = json["points per enemy killed"].AsInt;
        pointsPerProjectilePunched = json["points per projectile punched"].AsInt;
        pointsPerFullHealthHealthKit = json["points per full health health kit"].AsInt;
    }

    private void FixedUpdate()
    {
        while (timerPointsPerSecond.TimeUp(Time.deltaTime))
        {
            textScore.AddValue(pointsPerSecondPlaying);
        }
    }

    public void Add(int val)
    {
        textScore.AddValue(val);
    }

    public int GetPointsPerEnemyKilled()
    {
        return pointsPerEnemyKilled;
    }
    public int GetPointsPerProjectilePunched()
    {
        return pointsPerProjectilePunched;
    }
    public int GetPointsPerFullHealthHealthKit()
    {
        return pointsPerFullHealthHealthKit;
    }
}