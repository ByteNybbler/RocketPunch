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
    [SerializeField]
    [Tooltip("How many seconds of cooldown occur after the end of the punch before a punch can happen again.")]
    float secondsOfPunchCooldown;

    Timer timerPunching = new Timer();
    Timer timerPunchCooldown = new Timer();

    // Whether or not the punch cooldown is occurring.
    bool punchIsCoolingDown = false;

    public void SetSecondsOfPunching(float val)
    {
        secondsOfPunching = val;
        timerPunching.SetTargetTime(secondsOfPunching);
    }
    public void SetSecondsOfPunchCooldown(float val)
    {
        secondsOfPunchCooldown = val;
        timerPunchCooldown.SetTargetTime(secondsOfPunchCooldown);
    }

    private void Start()
    {
        punchingObject.SetActive(false);
        timerPunching.SetTargetTime(secondsOfPunching);
        timerPunchCooldown.SetTargetTime(secondsOfPunchCooldown);
    }

    public void Punch()
    {
        if (!IsPunching() && !punchIsCoolingDown)
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
                EndPunch();
            }
        }
        if (punchIsCoolingDown)
        {
            while (timerPunchCooldown.TimeUp(Time.deltaTime))
            {
                punchIsCoolingDown = false;
            }
        }
    }

    private void EndPunch()
    {
        punchingObject.SetActive(false);
        punchIsCoolingDown = true;
    }
}