// Author(s): Paul Calande
// Script for enemy movement in Rocket Puncher.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Reference to the Mover component.")]
    Mover2D mover;
    [SerializeField]
    [Tooltip("How quickly the enemy moves to the left.")]
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