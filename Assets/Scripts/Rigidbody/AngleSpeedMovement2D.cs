// Author(s): Paul Calande
// Moves the GameObject based on a given speed and angle.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngleSpeedMovement2D : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Reference to the Mover component.")]
    Mover2D mover;

    // The vector determining the direction and distance to move.
    Vector2 moveVector;

    private void FixedUpdate()
    {
        mover.MovePosition(moveVector * Time.deltaTime);
    }

    private Vector2 MakeAngleSpeedVector(float angle, float speed)
    {
        return Quaternion.AngleAxis(angle, Vector3.forward) * Vector2.right * speed;
    }

    // Call this method to change the GameObject's direction and speed.
    public void SetAngleSpeed(float angle, float speed)
    {
        moveVector = MakeAngleSpeedVector(angle, speed);
    }
}