// Author(s): Paul Calande
// Player input class for Rocket Puncher.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour, IPlayable
{
    [System.Serializable]
    public class Data
    {
        [Tooltip("Rocket movement speed.")]
        public float movementSpeed;

        public Data(float movementSpeed)
        {
            this.movementSpeed = movementSpeed;
        }
    }
    [SerializeField]
    Data data;

    [SerializeField]
    [Tooltip("Reference to the Mover component.")]
    Mover2D mover;
    [SerializeField]
    [Tooltip("Reference to the PlayerPunch component.")]
    PlayerPunch playerPunch;
    [SerializeField]
    TimeScale tsGameplay;
    [SerializeField]
    TimeScale tsBackground;
    [SerializeField]
    [Tooltip("Reference to the pause menu.")]
    GameObject pauseMenu;

    public void SetData(Data val)
    {
        data = val;
    }

    private void Start()
    {
        ServiceLocator.GetInputManager().AddSubscriber(this);
        pauseMenu.SetActive(false);
    }

    public void TogglePause()
    {
        if (tsGameplay.IsFrozen())
        {
            tsGameplay.SetTimeScale(1.0f);
            tsBackground.SetTimeScale(1.0f);
            pauseMenu.SetActive(false);
        }
        else
        {
            tsGameplay.SetTimeScale(0.0f);
            tsBackground.SetTimeScale(0.0f);
            pauseMenu.SetActive(true);
        }
    }

    public void ReceiveInput(InputReader inputReader)
    {
        float axisH = inputReader.GetAxisHorizontalRaw();
        float axisV = inputReader.GetAxisVerticalRaw();
        Vector2 change = new Vector2(axisH, axisV)
            * data.movementSpeed * tsGameplay.DeltaTime();
        mover.MovePosition(change);

        if (!tsGameplay.IsFrozen())
        {
            if (inputReader.GetKeyDown(KeyCode.Space))
            {
                playerPunch.Punch();
            }
        }

        if (inputReader.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }
}