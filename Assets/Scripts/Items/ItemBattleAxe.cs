// Author(s): Paul Calande
// Script for collectible Battle Axe item in Rocket Puncher.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBattleAxe : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerSelfHitbox"))
        {
            Transform root = collision.transform.root;
            PlayerPowerup playerPowerup = root.GetComponent<PlayerPowerup>();
            playerPowerup.GivePowerup(ItemType.BattleAxe);
            Destroy(gameObject);
        }
    }
}