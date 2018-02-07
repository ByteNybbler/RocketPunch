// Author(s): Paul Calande
// Script for the player's punching mechanic in Rocket Puncher.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPunch : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The GameObject to use for punching.")]
    GameObject punchingObject;
    [SerializeField]
    [Tooltip("How many seconds the punch lasts for.")]
    float secondsOfPunching;

    Timer timerPunching;

    private void Start()
    {
        punchingObject.SetActive(false);
        timerPunching = new Timer(secondsOfPunching);
    }

    public void Punch()
    {
        if (!IsPunching())
        {
            punchingObject.SetActive(true);
        }
    }

    // Returns true if the player is punching.
    private bool IsPunching()
    {
        return punchingObject.activeInHierarchy;
    }

    private void FixedUpdate()
    {
        if (IsPunching())
        {
            while (timerPunching.TimeUp(Time.deltaTime))
            {
                punchingObject.SetActive(false);
            }
        }
    }
}