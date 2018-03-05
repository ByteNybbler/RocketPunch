// Author(s): Paul Calande
// Convenient script for scaling time on a per-GameObject basis.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeScale : MonoBehaviour
{
    // The multiplier for time.
    float timeScale = 1.0f;

    // Returns delta time, taking the time scale into account.
    public float DeltaTime()
    {
        return Time.deltaTime * timeScale;
    }

    public void SetTimeScale(float val)
    {
        timeScale = val;
    }

    public float GetTimeScale()
    {
        return timeScale;
    }

    // Returns true if time is frozen for this time scale.
    public bool IsFrozen()
    {
        return timeScale == 0.0f;
    }
}