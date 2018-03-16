// Author(s): Paul Calande
// Script for tuning the player.

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
        public PlayerHealth.Data healthData;
        public PlayerDeathTracker.Data deathTrackerData;

        public Data(PlayerPunch.Data punchData,
            PlayerPowerup.Data powerupData,
            PlayerInput.Data inputData,
            PlayerHealth.Data healthData,
            PlayerDeathTracker.Data deathTrackerData)
        {
            this.punchData = punchData;
            this.powerupData = powerupData;
            this.inputData = inputData;
            this.healthData = healthData;
            this.deathTrackerData = deathTrackerData;
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
    [Tooltip("Reference to the PlayerDeathTracker component.")]
    PlayerDeathTracker playerDeathTracker;

    [SerializeField]
    [Tooltip("Voice clips for starting the level.")]
    SOAAudioClip voiceStart;

    AudioController ac;

    public void SetData(Data val)
    {
        playerPunch.SetData(val.punchData);
        playerPowerup.SetData(val.powerupData);
        playerInput.SetData(val.inputData);
        playerHealth.SetData(val.healthData);
        playerDeathTracker.SetData(val.deathTrackerData);
    }

    private void Awake()
    {
        Tune();
    }

    private void Start()
    {
        ac = ServiceLocator.GetAudioController();

        ac.PlaySFX(voiceStart.GetRandomElement());
    }

    private void Tune()
    {
        JSONNodeReader jsonP = new JSONNodeReader(playerFile);
        JSONNodeReader jsonI = new JSONNodeReader(itemFile);
        PlayerPunch.Data punchData = new PlayerPunch.Data(
            jsonP.Get("seconds of punching", 1.0f),
            jsonP.Get("seconds of punch cooldown", 1.0f),
            jsonI.Get("seconds of more arms punch cooldown", 0.1f),
            jsonP.Get("base punch hitbox size", 1.4f),
            jsonI.Get("battle axe hitbox size", 2.0f));
        PlayerPowerup.Data powerupData = new PlayerPowerup.Data(
            jsonI.Get("seconds of battle axe", 8.0f),
            jsonI.Get("seconds of more arms", 8.0f));
        PlayerInput.Data inputData = new PlayerInput.Data(
            jsonP.Get("base movement speed", 10.0f));
        PlayerHealth.Data healthData = new PlayerHealth.Data(
            jsonP.Get("seconds of invincibility when damaged", 1.0f),
            jsonP.Get("chance of saying fuck", 0.01f),
            jsonP.Get("max health", 100));
        PlayerDeathTracker.Data deathData = new PlayerDeathTracker.Data(
            jsonP.Get("seconds to wait after dying", 5.0f));
        SetData(new Data(punchData, powerupData, inputData, healthData, deathData));
    }
}