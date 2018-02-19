// Author(s): Paul Calande
// Enemy health class for Rocket Puncher.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [System.Serializable]
    public class Data
    {
        [Tooltip("The health kit to drop when dead.")]
        public ItemHealthKit.Data healthKit;
        [Tooltip("How many points the enemy gives when it's killed.")]
        public int pointsWhenKilled;
        [Tooltip("Item drop rates.")]
        public Probability<ItemType> probItem;
        [Tooltip("Reference to the Score instance.")]
        public Score score;

        public Data(ItemHealthKit.Data healthKit, int pointsWhenKilled,
            Probability<ItemType> probItem, Score score)
        {
            this.healthKit = healthKit;
            this.pointsWhenKilled = pointsWhenKilled;
            this.probItem = probItem;
            this.score = score;
        }
    }
    [SerializeField]
    [Tooltip("Data for EnemyHealth.")]
    Data data;

    [SerializeField]
    [Tooltip("Reference to the enemy's LeftMovement component.")]
    LeftMovement leftMovement;
    [SerializeField]
    [Tooltip("The prefab to use to spawn health kits.")]
    GameObject prefabHealthKit;
    [SerializeField]
    [Tooltip("The prefab to use to spawn the Battle Axe powerup.")]
    GameObject prefabBattleAxe;
    [SerializeField]
    [Tooltip("The prefab to use to spawn the More Arms powerup.")]
    GameObject prefabMoreArms;

    PlayerPowerup playerPowerup;

    public void SetData(Data val)
    {
        data = val;
    }

    private void Awake()
    {
        GameObject player = ServiceLocator.GetPlayer();
        playerPowerup = player.GetComponent<PlayerPowerup>();
    }

    public void Kill()
    {
        data.score.Add(data.pointsWhenKilled);
        DropItem();
        Destroy(gameObject);
    }

    // Attempt to drop something.
    private void DropItem()
    {
        ItemType it = data.probItem.Roll();
        if (it == ItemType.None)
        {
            return;
        }
        if (it == ItemType.HealthKit)
        {
            GameObject pup = Instantiate(prefabHealthKit, transform.position, Quaternion.identity);
            ItemHealthKit hk = pup.GetComponent<ItemHealthKit>();
            hk.SetData(data.healthKit);
            LeftMovement lm = pup.GetComponent<LeftMovement>();
            lm.SetMovementLeftSpeed(leftMovement.GetMovementLeftSpeed());
        }
        else
        {
            // Only drop these powerups if none currently exist.
            if (!playerPowerup.GetPowerupExists())
            {
                GameObject powerup = null;
                switch (it)
                {
                    case ItemType.BattleAxe:
                        powerup = Instantiate(prefabBattleAxe, transform.position, Quaternion.identity);
                        break;
                    case ItemType.MoreArms:
                        powerup = Instantiate(prefabMoreArms, transform.position, Quaternion.identity);
                        break;
                }
                ItemPowerup pup = powerup.GetComponent<ItemPowerup>();
                pup.SetPlayerPowerup(playerPowerup);
                LeftMovement lm = powerup.GetComponent<LeftMovement>();
                lm.SetMovementLeftSpeed(leftMovement.GetMovementLeftSpeed());
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerPunch"))
        {
            Kill();
        }
    }
}