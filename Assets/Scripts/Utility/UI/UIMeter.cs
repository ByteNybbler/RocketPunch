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

    // Fills the meter up to the given percentage.
    public void SetPercent(float percent)
    {
        // Cap the percent at 100% so that the anchors don't overflow.
        if (percent > 1.0f)
        {
            percent = 1.0f;
        }

        if (invisibleWhenEmpty)
        {
            if (percent <= 0.0f)
            {
                SetMeterVisible(false);
            }
            else
            {
                SetMeterVisible(true);
            }
        }
        if (invisibleWhenFull)
        {
            if (percent >= 1.0f)
            {
                SetMeterVisible(false);
            }
            else
            {
                SetMeterVisible(true);
            }
        }

        // Adjust anchors.
        Vector2 newAnchorMax = meterFront.anchorMax;
        newAnchorMax.x = percent;
        meterFront.anchorMax = newAnchorMax;
    }

    // Fills the meter up to the calculated percentage.
    public void SetProportion(float currentValue, float maxValue)
    {
        SetPercent(currentValue / maxValue);
    }

    // Set whether the meter is visible or not.
    private void SetMeterVisible(bool visible)
    {
        gameObject.SetActive(visible);
    }
}