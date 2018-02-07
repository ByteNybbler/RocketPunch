// Author(s): Paul Calande
// Class for manipulating UI meters.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMeter : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Reference to the RectTransform of the front component of the meter.")]
    RectTransform meterFront;

    // Fills the meter up to the given percentage.
    public void SetPercent(float percent)
    {
        Vector2 newAnchorMax = meterFront.anchorMax;
        newAnchorMax.x = percent;
        meterFront.anchorMax = newAnchorMax;
    }

    // Fills the meter up to the calculated percentage.
    public void SetProportion(float currentValue, float maxValue)
    {
        SetPercent(currentValue / maxValue);
    }
}