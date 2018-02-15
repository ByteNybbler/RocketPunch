// Author(s): Paul Calande
// Script for moving a GameObject to a relative position or rotation.
//
// Useful if the user needs to call Rigidbody.MovePosition multiple times in one
// FixedUpdate step, since only the latest MovePosition call is considered by
// the physics engine. Same for MoveRotation.

// -=-=-=-=-=-=- !!! IMPORTANT !!! -=-=-=-=-=-=-
//
// Make sure to set this script's FixedUpdate call to be past the default time in the
// Script Execution Order Settings!
//
// -=-=-=-=-=-=- !!! IMPORTANT !!! -=-=-=-=-=-=-

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover2D : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Reference to the Rigidbody component to move.")]
    Rigidbody2D myRigidbody;

    Vector2 differencePosition = Vector2.zero;
    float differenceRotation = 0.0f;

    public void MovePosition(Vector2 change)
    {
        differencePosition += change;
    }

    public void MoveRotation(float change)
    {
        differenceRotation += change;
    }

    public void TeleportPosition(Vector2 newPos)
    {
        myRigidbody.position = newPos;
    }

    public void TeleportRotation(float newRotation)
    {
        myRigidbody.rotation = newRotation;
    }

    public Vector2 GetPosition()
    {
        return myRigidbody.position;
    }

    public float GetRotation()
    {
        return myRigidbody.rotation;
    }

    private void FixedUpdate()
    {
        myRigidbody.MovePosition(myRigidbody.position + differencePosition);
        myRigidbody.MoveRotation(myRigidbody.rotation + differenceRotation);
        differencePosition = Vector2.zero;
        differenceRotation = 0.0f;
    }
}