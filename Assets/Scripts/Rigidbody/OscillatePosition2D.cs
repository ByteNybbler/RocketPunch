// Author(s): Paul Calande
// Modify a GameObject's position based on sinusoidal curves.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OscillatePosition2D : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Reference to the Mover component.")]
    Mover2D mover;
    [SerializeField]
    [Tooltip("The size of the x oscillation.")]
    float xMagnitude;
    [SerializeField]
    [Tooltip("The speed of the x oscillation.")]
    float xSpeed;
    [SerializeField]
    [Tooltip("The size of the y oscillation.")]
    float yMagnitude;
    [SerializeField]
    [Tooltip("The speed of the y oscillation.")]
    float ySpeed;

    Oscillator oscX;
    Oscillator oscY;

    public void Init(float xMagnitude, float xSpeed, float yMagnitude, float ySpeed)
    {
        this.xMagnitude = xMagnitude;
        this.xSpeed = xSpeed;
        this.yMagnitude = yMagnitude;
        this.ySpeed = ySpeed;
    }

    private void Start()
    {
        oscX = new Oscillator(xMagnitude, xSpeed, Mathf.Sin);
        oscY = new Oscillator(yMagnitude, ySpeed, Mathf.Sin);
    }

    private void FixedUpdate()
    {
        float xDifference = oscX.SampleDelta(Time.deltaTime);
        float yDifference = oscY.SampleDelta(Time.deltaTime);
        Vector2 change = new Vector2(xDifference, yDifference);
        //Vector2 newPos = rb.position + change;
        //rb.MovePosition(newPos);
        mover.MovePosition(change);
    }
}