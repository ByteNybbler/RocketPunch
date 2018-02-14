// Author(s): Paul Calande
// Script for constant leftwards movement.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftMovement : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Reference to the Mover component.")]
    Mover2D mover;
    [SerializeField]
    [Tooltip("How quickly the GameObject moves to the left.")]
    float movementLeftSpeed;

    private void FixedUpdate()
    {
        Vector2 change = new Vector2(-movementLeftSpeed, 0.0f);
        mover.MovePosition(change);
    }

    public void SetMovementLeftSpeed(float value)
    {
        movementLeftSpeed = value;
    }
}