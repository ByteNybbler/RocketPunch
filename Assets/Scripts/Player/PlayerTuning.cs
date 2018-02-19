using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

public class PlayerTuning : MonoBehaviour
{
    [SerializeField]
    [Tooltip("File to use for player tuning variables.")]
    TextAsset playerFile;
    [SerializeField]
    [Tooltip("File to use for item tuning variables.")]
    TextAsset itemFile;
    [SerializeField]
    [Tooltip("Reference to the PlayerPunch component.")]
    PlayerPunch playerPunch;
    [SerializeField]
    [Tooltip("Reference to the PlayerHealth component.")]
    PlayerHealth playerHealth;
    [SerializeField]
    [Tooltip("Reference to the PlayerPowerup component.")]
    PlayerPowerup playerPowerup;
    [SerializeField]
    [Tooltip("Reference to the Health component.")]
    Health health;

    private void Awake()
    {
        Tune();
    }

    private void Tune()
    {
        JSONNode jsonP = JSON.Parse(playerFile.ToString());
        JSONNode jsonI = JSON.Parse(itemFile.ToString());
        PlayerPunch.Data punchData = new PlayerPunch.Data(jsonP["seconds of punching"].AsFloat,
            jsonP["seconds of punch cooldown"].AsFloat,
            jsonP["base punch hitbox size"].AsFloat,
            jsonI["battle axe hitbox size"].AsFloat);
        playerPunch.SetData(punchData);
        PlayerPowerup.Data powerupData = new PlayerPowerup.Data(jsonI["seconds of battle axe"].AsFloat,
            jsonI["seconds of more arms"].AsFloat);
        playerPowerup.SetData(powerupData);
        playerHealth.SetSecondsOfInvincibilityWhenDamaged(jsonP["seconds of invincibility when damaged"].AsFloat);
        health.SetMaxHealth(jsonP["max health"].AsInt);
        health.FullHeal();
    }
}