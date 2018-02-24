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
        [Tooltip("The scale of the base punch's hitbox.")]
        public float basePunchHitboxSize;
        [Tooltip("The scale of the Battle Axe hitbox.")]
        public float battleAxeHitboxSize;

        public Data(float secondsOfPunching, float secondsOfPunchCooldown,
            float basePunchHitboxSize, float battleAxeHitboxSize)
        {
            this.secondsOfPunching = secondsOfPunching;
            this.secondsOfPunchCooldown = secondsOfPunchCooldown;
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

    Timer timerPunching = new Timer();
    Timer timerPunchCooldown = new Timer();

    // Whether or not the punch cooldown is occurring.
    bool punchIsCoolingDown = false;

    public void SetData(Data val)
    {
        data = val;
    }

    private void Start()
    {
        punchingObject.SetActive(false);
        UseBattleAxe(false);
        timerPunching.SetTargetTime(data.secondsOfPunching);
        timerPunchCooldown.SetTargetTime(data.secondsOfPunchCooldown);
        childRegularPunch.transform.localScale = new Vector3(data.basePunchHitboxSize,
            data.basePunchHitboxSize, 1.0f);
        childBattleAxe.transform.localScale = new Vector3(data.battleAxeHitboxSize,
            data.battleAxeHitboxSize, 1.0f);
    }

    public void SetSecondsOfPunching(float val)
    {
        data.secondsOfPunching = val;
        timerPunching.SetTargetTime(data.secondsOfPunching);
    }

    public void SetSecondsOfPunchCooldown(float val)
    {
        data.secondsOfPunchCooldown = val;
        timerPunchCooldown.SetTargetTime(data.secondsOfPunchCooldown);
    }

    public void UseBattleAxe(bool willUseAxe)
    {
        childRegularPunch.SetActive(!willUseAxe);
        childBattleAxe.SetActive(willUseAxe);
    }

    // Returns true if the player currently has the Battle Axe.
    public bool HasBattleAxe()
    {
        return childBattleAxe.activeSelf;
    }

    public void Punch()
    {
        if (!IsPunching() && !punchIsCoolingDown)
        {
            punchingObject.SetActive(true);
        }
    }

    // Returns true if the player is punching.
    private bool IsPunching()
    {
        return punchingObject.activeSelf;
    }

    private void FixedUpdate()
    {
        if (IsPunching())
        {
            while (timerPunching.TimeUp(Time.deltaTime))
            {
                EndPunch();
            }
        }
        if (punchIsCoolingDown)
        {
            while (timerPunchCooldown.TimeUp(Time.deltaTime))
            {
                punchIsCoolingDown = false;
            }
        }
    }

    private void EndPunch()
    {
        punchingObject.SetActive(false);
        punchIsCoolingDown = true;
    }
}