// Author(s): Paul Calande
// Player input class for Rocket Puncher.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour, IInputSubscriber
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
        float deltaX = axisH * movementSpeed;
        float deltaY = axisV * movementSpeed;
        //Vector3 change = new Vector2(deltaX, deltaY);
        //Vector2 newPos = transform.position + change;
        Vector2 change = new Vector2(deltaX, deltaY) * Time.deltaTime;
        Vector2 newPos = rb.position + change;

        rb.MovePosition(newPos);
    }
}