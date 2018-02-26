// Author(s): Paul Calande
// Script for the player's punching mechanic in Rocket Puncher.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPunch : MonoBehaviour
{
    [System.Serializable]
    public class Data
    {
        [Tooltip("How many seconds the punch lasts for.")]
        public float secondsOfPunching;
        [Tooltip("How many seconds of cooldown occur after the end of the punch before a punch can happen again.")]
        public float secondsOfPunchCooldown;
        [Tooltip("Punch cooldown during More Arms powerup.")]
        public float secondsOfMoreArmsPunchCooldown;
        [Tooltip("The scale of the base punch's hitbox.")]
        public float basePunchHitboxSize;
        [Tooltip("The scale of the Battle Axe hitbox.")]
        public float battleAxeHitboxSize;

        public Data(float secondsOfPunching, float secondsOfPunchCooldown,
            float secondsOfMoreArmsPunchCooldown,
            float basePunchHitboxSize, float battleAxeHitboxSize)
        {
            this.secondsOfPunching = secondsOfPunching;
            this.secondsOfPunchCooldown = secondsOfPunchCooldown;
            this.secondsOfMoreArmsPunchCooldown = secondsOfMoreArmsPunchCooldown;
            this.basePunchHitboxSize = basePunchHitboxSize;
            this.battleAxeHitboxSize = battleAxeHitboxSize;
        }
    }
    [SerializeField]
    Data data;

    [SerializeField]
    [Tooltip("The GameObject to use for punching.")]
    GameObject punchingObject;
    [SerializeField]
    [Tooltip("The punching object child to use for regular punching.")]
    GameObject childRegularPunch;
    [SerializeField]
    [Tooltip("The punching object child to use for the Battle Axe powerup.")]
    GameObject childBattleAxe;
    [SerializeField]
    [Tooltip("Voice clips for punching.")]
    SOAAudioClip punchVoiceClips;

    Timer timerPunching;
    Timer timerPunchCooldown;

    AudioController ac;

    public void SetData(Data val)
    {
        data = val;
    }

    private void Start()
    {
        ac = ServiceLocator.GetAudioController();
        punchingObject.SetActive(false);
        UseBattleAxe(false);

        childRegularPunch.transform.localScale = new Vector3(data.basePunchHitboxSize,
            data.basePunchHitboxSize, 1.0f);
        childBattleAxe.transform.localScale = new Vector3(data.battleAxeHitboxSize,
            data.battleAxeHitboxSize, 1.0f);

        timerPunching = new Timer(data.secondsOfPunching, false, false);
        timerPunchCooldown = new Timer(data.secondsOfPunchCooldown, false, false);
    }

    public void UseBattleAxe(bool willUseAxe)
    {
        childRegularPunch.SetActive(!willUseAxe);
        childBattleAxe.SetActive(willUseAxe);
    }

    public void UseMoreArms(bool willUseMoreArms)
    {
        float cooldown;
        if (willUseMoreArms)
        {
            cooldown = data.secondsOfMoreArmsPunchCooldown;
        }
        else
        {
            cooldown = data.secondsOfPunchCooldown;
        }
        timerPunchCooldown.SetTargetTime(cooldown);
    }

    // Returns true if the player currently has the Battle Axe.
    public bool HasBattleAxe()
    {
        return childBattleAxe.activeSelf;
    }

    public void Punch()
    {
        if (!IsPunching() && !IsPunchCoolingDown())
        {
            punchingObject.SetActive(true);
            ac.PlaySFX(punchVoiceClips.GetRandomElement());
            timerPunching.Start();
        }
    }

    // Returns true if the player is punching.
    private bool IsPunching()
    {
        return timerPunching.IsRunning();
    }

    // Returns true during the punch cooldown.
    private bool IsPunchCoolingDown()
    {
        return timerPunchCooldown.IsRunning();
    }

    private void FixedUpdate()
    {
        while (timerPunching.TimeUp(Time.deltaTime))
        {
            // Punch finished.
            punchingObject.SetActive(false);
            timerPunchCooldown.Start();
        }
        while (timerPunchCooldown.TimeUp(Time.deltaTime))
        {
            // Punch cooldown finished.
        }
    }
}