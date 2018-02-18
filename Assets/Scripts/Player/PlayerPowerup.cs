// Author(s): Paul Calande
//

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPowerup : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Reference to the PlayerPunch component.")]
    PlayerPunch playerPunch;

    // Whether a collectible powerup currently exists in the scene.
    // Is also true if a powerup effect is currently active.
    bool powerupExists = false;

    // Timer for the Battle Axe powerup effect.
    Timer timerBattleAxe = new Timer();

    public bool GetPowerupExists()
    {
        return powerupExists;
    }
    public void SetPowerupExists(bool val)
    {
        powerupExists = val;
    }

    public void GivePowerup(ItemType itemType)
    {
        switch (itemType)
        {
            case ItemType.BattleAxe:
                playerPunch.UseBattleAxe(true);
                break;
        }
    }

    private void FixedUpdate()
    {
        if (playerPunch.HasBattleAxe())
        {
            while (timerBattleAxe.TimeUp(Time.deltaTime))
            {
                playerPunch.UseBattleAxe(false);
            }
        }
    }
}