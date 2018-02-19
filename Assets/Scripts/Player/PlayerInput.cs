// Author(s): Paul Calande
// Player input class for Rocket Puncher.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour, IPlayable
{
    [SerializeField]
    [Tooltip("Reference to the Mover component.")]
    Mover2D mover;
    [SerializeField]
    [Tooltip("Reference to the PlayerPunch component.")]
    PlayerPunch playerPunch;
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
        Vector2 change = new Vector2(deltaX, deltaY) * Time.deltaTime;
        mover.MovePosition(change);

        if (inputData.GetKeyDown(KeyCode.Space))
        {
            playerPunch.Punch();
        }
    }
}