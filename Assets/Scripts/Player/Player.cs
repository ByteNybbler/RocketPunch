using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public class Data
    {
        public PlayerPunch.Data punchData;
        public PlayerPowerup.Data powerupData;
        public PlayerInput.Data inputData;

        public Data(PlayerPunch.Data punchData,
            PlayerPowerup.Data powerupData,
            PlayerInput.Data inputData)
        {
            this.punchData = punchData;
            this.powerupData = powerupData;
            this.inputData = inputData;
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
    [Tooltip("Reference to the PlayerInput component.")]
    PlayerInput playerInput;
    [SerializeField]
    [Tooltip("Reference to the Health component.")]
    Health health;

    public void SetData(Data val)
    {
        playerPunch.SetData(val.punchData);
        playerPowerup.SetData(val.powerupData);
        playerInput.SetData(val.inputData);
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
        PlayerInput.Data inputData = new PlayerInput.Data(
            jsonP.TryGetFloat("base movement speed", 10.0f));
        SetData(new Data(punchData, powerupData, inputData));
    }
}