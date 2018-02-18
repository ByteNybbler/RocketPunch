// Author(s): Paul Calande
// Script for constant leftwards movement.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftMovement : MonoBehaviour
{
    [System.Serializable]
    public class Data
    {
        [Tooltip("How quickly the GameObject moves to the left.")]
        public float movementLeftSpeed;
    }
    [SerializeField]
    Data data;

    [SerializeField]
    [Tooltip("Reference to the Mover component.")]
    Mover2D mover;

    public void SetData(Data val)
    {
        data = val;
    }

    private void FixedUpdate()
    {
        Vector2 change = new Vector2(-data.movementLeftSpeed, 0.0f);
        mover.MovePosition(change);
    }

    public void SetMovementLeftSpeed(float value)
    {
        data.movementLeftSpeed = value;
    }

    public float GetMovementLeftSpeed()
    {
        return data.movementLeftSpeed;
    }
}