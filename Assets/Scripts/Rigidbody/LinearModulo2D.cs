// Author(s): Paul Calande
// Modulo-based linear movement, for repeating patterns.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinearModulo2D : MonoBehaviour
{
    public class Data
    {
        [Tooltip("The movement vector to use.")]
        public Vector2 moveVector;
        [Tooltip("The distance to cover before returning to the original position.")]
        public float moduloDistance;

        public Data(Vector2 moveVector, float moduloDistance)
        {
            this.moveVector = moveVector;
            this.moduloDistance = moduloDistance;
        }
    }
    [SerializeField]
    Data data;

    [SerializeField]
    [Tooltip("Reference to the mover.")]
    Mover2D mover;

    float vectorMagnitude;
    float distanceCovered = 0.0f;
    //Vector2 moveVectorNormalized;
    Vector2 startPos;

    public void SetData(Data val)
    {
        data = val;
        vectorMagnitude = data.moveVector.magnitude;
        //moveVectorNormalized = data.moveVector.normalized;
        startPos = mover.GetPosition();
    }

    /*
    private void Start()
    {
        vectorMagnitude = data.moveVector.magnitude;
        moveVectorNormalized = data.moveVector.normalized;
    }
    */

    private void FixedUpdate()
    {
        mover.MovePosition(data.moveVector * Time.deltaTime);
        distanceCovered += vectorMagnitude * Time.deltaTime;
        while (distanceCovered >= data.moduloDistance)
        {
            distanceCovered -= data.moduloDistance;
            //mover.MovePosition(-moveVectorNormalized * data.moduloDistance);
            mover.TeleportPosition(startPos);
        }
    }
}