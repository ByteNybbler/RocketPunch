// Author(s): Paul Calande
// Convenient script for scaling time on a per-GameObject basis.
// Note that it uses FixedUpdate's time step.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeScale : MonoBehaviour
{
    // The multiplier for time.
    float timeScale = 1.0f;
    // Whether the time scale is paused.
    // Pausing the time scale effectively sets delta time to zero.
    bool paused = false;
    // Stores the delta time based on the time scale settings.
    // Helps avoid unnecessary repeated calculations.
    float cachedDeltaTime;

    private void Start()
    {
        CalculateDeltaTime();
    }

    // Update the cached delta time value based on the time scale settings.
    private void CalculateDeltaTime()
    {
        if (paused)
        {
            cachedDeltaTime = 0.0f;
        }
        else
        {
            cachedDeltaTime = Time.fixedDeltaTime * timeScale;
        }
    }

    // Returns delta time, taking the time scale into account.
    // Returns zero if the time scale is paused.
    public float DeltaTime()
    {
        return cachedDeltaTime;
    }

    public void SetTimeScale(float val)
    {
        timeScale = val;
        CalculateDeltaTime();
    }

    public float GetTimeScale()
    {
        return timeScale;
    }

    // Toggles whether the time scale is paused, effectively setting the time scale to zero.
    // This method is intended to be called by pause menus.
    // Do not use this for "time stop" gameplay effects, as it does.
    public void TogglePause()
    {
        paused = !paused;
        CalculateDeltaTime();
    }

    // Returns true if the time scale is paused.
    public bool IsPaused()
    {
        return paused;
    }

    // Returns true if time is frozen for this time scale.
    // To be used to check whether time has been stopped during gameplay.
    // Will not necessarily return true if the paused variable is true.
    public bool IsFrozen()
    {
        return timeScale == 0.0f;
    }
}