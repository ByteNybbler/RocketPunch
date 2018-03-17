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
    [SerializeField]
    [Tooltip("Whether the meter should be invisible when it becomes 100% filled.")]
    bool invisibleWhenFull;
    [SerializeField]
    [Tooltip("Whether the meter should be invisible when it becomes 0% filled.")]
    bool invisibleWhenEmpty;

    // If true, the meter will always be invisible.
    bool forcedInvisible = false;

    // Fills the meter up to the given percentage.
    public void SetPercent(float percent)
    {
        // Cap the percent at 100% so that the anchors don't overflow.
        if (percent > 1.0f)
        {
            percent = 1.0f;
        }

        // Adjust anchors.
        Vector2 newAnchorMax = meterFront.anchorMax;
        newAnchorMax.x = percent;
        meterFront.anchorMax = newAnchorMax;

        CheckMeterVisibility();
    }

    // Fills the meter up to the calculated percentage based on the given proportion.
    public void SetProportion(float currentValue, float maxValue)
    {
        SetPercent(currentValue / maxValue);
    }

    // Returns the current percent of the meter.
    public float GetCurrentPercent()
    {
        return meterFront.anchorMax.x;
    }

    // Makes the meter visible or invisible based on the current percentage.
    private void CheckMeterVisibility()
    {
        if (forcedInvisible)
        {
            SetMeterVisible(false);
            return;
        }
        float percent = GetCurrentPercent();
        if (percent <= 0.0f)
        {
            // The meter is empty.
            if (invisibleWhenEmpty)
            {
                SetMeterVisible(false);
            }
        }
        else if (percent >= 1.0f)
        {
            // The meter is full.
            if (invisibleWhenFull)
            {
                SetMeterVisible(false);
            }
        }
        else
        {
            // The meter is not empty or full.
            SetMeterVisible(true);
        }
    }

    // Turns forced invisibility on or off.
    public void SetForcedInvisible(bool invisible)
    {
        forcedInvisible = invisible;
        CheckMeterVisibility();
    }

    // Makes the meter visible or invisible.
    private void SetMeterVisible(bool visible)
    {
        gameObject.SetActive(visible);
    }
}