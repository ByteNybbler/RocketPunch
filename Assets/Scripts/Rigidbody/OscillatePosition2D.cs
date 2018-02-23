// Author(s): Paul Calande
// Modify a GameObject's position based on sinusoidal curves.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OscillatePosition2D : MonoBehaviour
{
    [System.Serializable]
    public class Data : IDeepCopyable<Data>
    {
        [Tooltip("The size of the x oscillation.")]
        public float xMagnitude;
        [Tooltip("The speed of the x oscillation.")]
        public float xSpeed;
        [Tooltip("The size of the y oscillation.")]
        public float yMagnitude;
        [Tooltip("The speed of the y oscillation.")]
        public float ySpeed;

        public Data(float xMagnitude, float xSpeed, float yMagnitude, float ySpeed)
        {
            this.xMagnitude = xMagnitude;
            this.xSpeed = xSpeed;
            this.yMagnitude = yMagnitude;
            this.ySpeed = ySpeed;
        }

        public Data DeepCopy()
        {
            return new Data(xMagnitude, xSpeed, yMagnitude, ySpeed);
        }
    }
    [SerializeField]
    Data data;

    [SerializeField]
    [Tooltip("Reference to the Mover component.")]
    Mover2D mover;

    Oscillator oscX;
    Oscillator oscY;

    public void SetData(Data val)
    {
        data = val;
    }

    private void Start()
    {
        oscX = new Oscillator(data.xMagnitude, data.xSpeed, Mathf.Sin);
        oscY = new Oscillator(data.yMagnitude, data.ySpeed, Mathf.Sin);
    }

    private void FixedUpdate()
    {
        float xDifference = oscX.SampleDelta(Time.deltaTime);
        float yDifference = oscY.SampleDelta(Time.deltaTime);
        Vector2 change = new Vector2(xDifference, yDifference);
        mover.MovePosition(change);
    }
}