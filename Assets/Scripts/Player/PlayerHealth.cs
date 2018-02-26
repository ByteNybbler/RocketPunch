// Author(s): Paul Calande
// Script for player health in Rocket Puncher.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [System.Serializable]
    public class Data
    {
        [Tooltip("How many seconds the player remains invincible when damaged.")]
        public float secondsOfInvincibilityWhenDamaged;
        [Tooltip("The chance that Rocket Puncher has to say fuck when damaged.")]
        public float chanceOfSayingFuck;
        [Tooltip("The maximum health that the player should have.")]
        public int maxHealth;

        public Data(float secondsOfInvincibilityWhenDamaged,
            float chanceOfSayingFuck, int maxHealth)
        {
            this.secondsOfInvincibilityWhenDamaged = secondsOfInvincibilityWhenDamaged;
            this.chanceOfSayingFuck = chanceOfSayingFuck;
            this.maxHealth = maxHealth;
        }
    }
    [SerializeField]
    Data data;

    [SerializeField]
    [Tooltip("Reference to the Health component.")]
    Health health;
    [SerializeField]
    [Tooltip("Reference to the renderer.")]
    SpriteRenderer render;
    [SerializeField]
    [Tooltip("The color alpha the player has when invincible.")]
    float damageAlpha;
    [SerializeField]
    [Tooltip("The player hurt voice clips.")]
    SOAAudioClip hurtVoiceClips;
    [SerializeField]
    [Tooltip("FUCK")]
    AudioClip fuck;
    [SerializeField]
    [Tooltip("The player heal voice clips.")]
    SOAAudioClip healVoiceClips;

    // Invincibility timer.
    Timer timerInvincible;
    // Probability machine for whether Rocket Puncher will say fuck.
    Probability<bool> playerSaysFuck = new Probability<bool>(false);

    AudioController ac;

    public void SetData(Data val)
    {
        data = val;
    }

    private void Start()
    {
        ac = ServiceLocator.GetAudioController();
        health.SetMaxHealth(data.maxHealth);
        health.FullHeal();
        health.Died += Health_Died;
        timerInvincible = new Timer(data.secondsOfInvincibilityWhenDamaged, false, false);
        playerSaysFuck.SetChance(true, data.chanceOfSayingFuck);
    }

    private void FixedUpdate()
    {
        while (timerInvincible.TimeUp(Time.deltaTime))
        {
            MakeVincible();
        }
    }

    private void Health_Died()
    {
        UtilScene.ResetScene();
    }

    // Returns whether the player can currently be damaged.
    private bool IsInvincible()
    {
        return timerInvincible.IsRunning();
    }

    // Play damage sounds.
    private void DamageAudio()
    {
        bool sayFuck = playerSaysFuck.Roll();
        if (sayFuck)
        {
            ac.PlaySFX(fuck);
        }
        else
        {
            ac.PlaySFX(hurtVoiceClips.GetRandomElement());
        }
    }

    public void Damage(int amount)
    {
        if (!IsInvincible())
        {
            health.Damage(amount);
            MakeInvincible();
            SetSpriteAlpha(damageAlpha);
            DamageAudio();
        }
    }

    public void HealAudio()
    {
        ac.PlaySFX(healVoiceClips.GetRandomElement());
    }

    public void Heal(int amount)
    {
        health.Heal(amount);
    }

    private void MakeVincible()
    {
        SetSpriteAlpha(1.0f);
    }

    private void MakeInvincible()
    {
        timerInvincible.Start();
    }

    private void SetSpriteAlpha(float alpha)
    {
        Color col = render.color;
        col.a = alpha;
        render.color = col;
    }
}