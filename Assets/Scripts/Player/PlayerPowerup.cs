// Author(s): Paul Calande
//

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPowerup : MonoBehaviour
{
    [System.Serializable]
    public class Data
    {
        [Tooltip("How many seconds the Battle Axe powerup lasts.")]
        public float secondsOfBattleAxe;
        [Tooltip("How many seconds the More Arms powerup lasts.")]
        public float secondsOfMoreArms;

        public Data(float secondsOfBattleAxe, float secondsOfMoreArms)
        {
            this.secondsOfBattleAxe = secondsOfBattleAxe;
            this.secondsOfMoreArms = secondsOfMoreArms;
        }
    }
    [SerializeField]
    Data data;

    [SerializeField]
    [Tooltip("Reference to the PlayerPunch component.")]
    PlayerPunch playerPunch;
    [SerializeField]
    [Tooltip("Voice clips for collecting a powerup.")]
    SOAAudioClip voicePowerup;

    AudioController ac;

    // Whether a collectible powerup currently exists in the scene.
    // Is also true if a powerup effect is currently active.
    bool powerupExists = false;

    // Timer for the Battle Axe powerup effect.
    Timer timerBattleAxe = new Timer();

    public void SetData(Data val)
    {
        data = val;
    }

    private void Start()
    {
        ac = ServiceLocator.GetAudioController();
        timerBattleAxe.SetTargetTime(data.secondsOfBattleAxe);
    }

    public bool GetPowerupExists()
    {
        return powerupExists;
    }
    public void SetPowerupExists(bool val)
    {
        powerupExists = val;
    }

    public void GivePowerup(ItemType itemType)
    {
        ac.PlaySFX(voicePowerup.GetRandomElement());
        switch (itemType)
        {
            case ItemType.BattleAxe:
                playerPunch.UseBattleAxe(true);
                break;
        }
    }

    private void FixedUpdate()
    {
        if (playerPunch.HasBattleAxe())
        {
            while (timerBattleAxe.TimeUp(Time.deltaTime))
            {
                playerPunch.UseBattleAxe(false);
                SetPowerupExists(false);
            }
        }
    }
}