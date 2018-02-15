// Author(s): Paul Calande
// Script for player health in Rocket Puncher.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Reference to the Health component.")]
    Health health;
    [SerializeField]
    [Tooltip("Reference to the renderer.")]
    SpriteRenderer render;
    [SerializeField]
    [Tooltip("The color alpha the player has when invincible.")]
    float damageAlpha;

    // Invincibility timer.
    Timer timerInvincible = new Timer();
    // Whether the player can currently be damaged.
    bool vincible = true;
    // Whether a powerup effect is currently active.
    bool powerupActive = false;

    private void Start()
    {
        health.Died += Health_Died;
    }

    public void SetSecondsOfInvincibilityWhenDamaged(float val)
    {
        timerInvincible.SetTargetTime(val);
    }

    public bool GetPowerupActive()
    {
        return powerupActive;
    }

    private void FixedUpdate()
    {
        if (!vincible)
        {
            while (timerInvincible.TimeUp(Time.deltaTime))
            {
                MakeVincible();
            }
        }
    }

    private void Health_Died()
    {
        UtilScene.ResetScene();
    }

    public void Damage(int amount)
    {
        if (vincible)
        {
            health.Damage(amount);
            MakeInvincible();
            SetSpriteAlpha(damageAlpha);
        }
    }

    public void Heal(int amount)
    {
        health.Heal(amount);
    }

    private void MakeVincible()
    {
        vincible = true;
        SetSpriteAlpha(1.0f);
    }

    private void MakeInvincible()
    {
        vincible = false;
    }

    private void SetSpriteAlpha(float alpha)
    {
        Color col = render.color;
        col.a = alpha;
        render.color = col;
    }
}