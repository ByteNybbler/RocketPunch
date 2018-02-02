// Author(s): Paul Calande
// A class built to contain input data for one FixedUpdate step.
// Key releases should be checked AFTER key presses when using this class.
// This is because a key release and a key press can happen on the same FixedUpdate frame.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputData
{
    float axisHorizontal = 0.0f;
    float axisVertical = 0.0f;
    float axisHorizontalRaw = 0.0f;
    float axisVerticalRaw = 0.0f;
    HashSet<KeyCode> keysDown = new HashSet<KeyCode>();
    HashSet<KeyCode> keysUp = new HashSet<KeyCode>();
    HashSet<KeyCode> keysHeld = new HashSet<KeyCode>();

    // Track the keypress data that is recorded each Update step.
    // To be called during Update.
    public void PopulateKeys()
    {
        foreach (KeyCode key in System.Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKey(key))
            {
                keysHeld.Add(key);
            }
            if (Input.GetKeyDown(key))
            {
                keysDown.Add(key);
            }
            if (Input.GetKeyUp(key))
            {
                keysUp.Add(key);
            }
        }
    }

    // Track the axis data.
    public void PopulateAxes()
    {
        axisHorizontal = Input.GetAxis("Horizontal");
        axisVertical = Input.GetAxis("Vertical");
        axisHorizontalRaw = Input.GetAxisRaw("Horizontal");
        axisVerticalRaw = Input.GetAxisRaw("Vertical");
    }

    // Resets input data for the next FixedUpdate step.
    // To be called after the input data is sent out.
    public void Clear()
    {
        keysDown.Clear();
        keysUp.Clear();
        keysHeld.Clear();
        axisHorizontal = 0.0f;
        axisVertical = 0.0f;
        axisHorizontalRaw = 0.0f;
        axisVerticalRaw = 0.0f;
    }

    public bool GetKey(KeyCode key)
    {
        return keysHeld.Contains(key);
    }

    public bool GetKeyDown(KeyCode key)
    {
        return keysDown.Contains(key);
    }

    public bool GetKeyUp(KeyCode key)
    {
        return keysUp.Contains(key);
    }

    public float GetAxisHorizontal()
    {
        return axisHorizontal;
    }

    public float GetAxisVertical()
    {
        return axisVertical;
    }

    public float GetAxisHorizontalRaw()
    {
        return axisHorizontalRaw;
    }

    public float GetAxisVerticalRaw()
    {
        return axisVerticalRaw;
    }
}