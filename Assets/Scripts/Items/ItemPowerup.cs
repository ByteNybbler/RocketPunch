// Author(s): Paul Calande
// General script to be attached to powerup items in Rocket Puncher.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPowerup : MonoBehaviour
{
    [SerializeField]
    [Tooltip("lol")]
    PlayerPowerup playerPowerup;

    public void SetPlayerPowerup(PlayerPowerup val)
    {
        playerPowerup = val;
    }

    private void Start()
    {
        // Notify the player that a powerup exists in the scene.
        playerPowerup.SetPowerupExists(true);
    }

    // The powerup no longer exists, so notify the player that there are no powerups left.
    private void OnDestroy()
    {
        if (playerPowerup != null)
        {
            playerPowerup.SetPowerupExists(false);
        }
    }
}