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
    [Tooltip("Reference to the PlayerPunch component.")]
    PlayerPunch playerPunch;
    [SerializeField]
    [Tooltip("Reference to the PlayerHealth component.")]
    PlayerHealth playerHealth;
    [SerializeField]
    [Tooltip("Reference to the Health component.")]
    Health health;

    private void Awake()
    {
        Tune();
    }

    private void Tune()
    {
        JSONNode json = JSON.Parse(playerFile.ToString());
        playerPunch.SetSecondsOfPunching(json["seconds of punching"].AsFloat);
        playerPunch.SetSecondsOfPunchCooldown(json["seconds of punch cooldown"].AsFloat);
        playerHealth.SetSecondsOfInvincibilityWhenDamaged(json["seconds of invincibility when damaged"].AsFloat);
        health.SetMaxHealth(json["max health"].AsInt);
        health.FullHeal();
    }
}