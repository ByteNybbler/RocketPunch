using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public class Data
    {
        public PlayerPunch.Data punchData;
        public PlayerPowerup.Data powerupData;

        public Data(PlayerPunch.Data punchData,
            PlayerPowerup.Data powerupData)
        {
            this.punchData = punchData;
            this.powerupData = powerupData;
        }
    }

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

    public void SetData(Data val)
    {
        playerPunch.SetData(val.punchData);
        playerPowerup.SetData(val.powerupData);
    }

    private void Awake()
    {
        Tune();
    }

    private void Tune()
    {
        JSONNodeReader jsonP = new JSONNodeReader(playerFile);
        JSONNodeReader jsonI = new JSONNodeReader(itemFile);
        PlayerPunch.Data punchData = new PlayerPunch.Data(
            jsonP.TryGetFloat("seconds of punching", 1.0f),
            jsonP.TryGetFloat("seconds of punch cooldown", 1.0f),
            jsonP.TryGetFloat("base punch hitbox size", 1.4f),
            jsonI.TryGetFloat("battle axe hitbox size", 2.0f));
        PlayerPowerup.Data powerupData = new PlayerPowerup.Data(
            jsonI.TryGetFloat("seconds of battle axe", 8.0f),
            jsonI.TryGetFloat("seconds of more arms", 8.0f));
        playerHealth.SetSecondsOfInvincibilityWhenDamaged(
            jsonP.TryGetFloat("seconds of invincibility when damaged", 1.0f));
        health.SetMaxHealth(jsonP.TryGetInt("max health", 100));
        health.FullHeal();
        SetData(new global::Player.Data(punchData, powerupData));
    }
}