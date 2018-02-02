// Author(s): Paul Calande
// Player class for Rocket Puncher.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IInputSubscriber
{
    [SerializeField]
    [Tooltip("Reference to the Rigidbody component.")]
    Rigidbody2D rb;
    [SerializeField]
    [Tooltip("Rocket movement speed.")]
    float movementSpeed;

    private void Start()
    {
        ServiceLocator.GetInputManager().AddSubscriber(this);
    }

    public void ReceiveInput(InputData inputData)
    {
        float axisH = inputData.GetAxisHorizontalRaw();
        float axisV = inputData.GetAxisVerticalRaw();
        float deltaX = axisH * movementSpeed * Time.deltaTime;
        float deltaY = axisV * movementSpeed * Time.deltaTime;
        Vector3 change = new Vector2(deltaX, deltaY);
        Vector2 newPos = transform.position + change;

        rb.MovePosition(newPos);
    }
}