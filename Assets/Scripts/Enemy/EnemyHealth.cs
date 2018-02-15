// Author(s): Paul Calande
// Enemy health class for Rocket Puncher.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
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

    [SerializeField]
    [Tooltip("Reference to the Score instance.")]
    Score score;
    [SerializeField]
    [Tooltip("How many points the enemy gives when it's killed.")]
    int pointsWhenKilled;
    [SerializeField]
    [Tooltip("How much health a health kit gives.")]
    int healthPerHealthKit;
    [SerializeField]
    [Tooltip("How many points the dropped health kit gives when the player is at full health.")]
    int pointsPerFullHealthHealthKit;

    // Item drop rates.
    Probability<ItemType> probItem;
    // Reference to the player's health.
    PlayerHealth playerHealth;

    private void Awake()
    {
        playerHealth = ServiceLocator.GetPlayer().GetComponent<PlayerHealth>();
    }

    public void SetScore(Score val)
    {
        score = val;
    }
    public void SetPointsWhenKilled(int val)
    {
        pointsWhenKilled = val;
    }
    public void SetHealthPerHealthKit(int val)
    {
        healthPerHealthKit = val;
    }
    public void SetPointsPerFullHealthHealthKit(int val)
    {
        pointsPerFullHealthHealthKit = val;
    }
    public void SetProbItem(Probability<ItemType> val)
    {
        probItem = val;
    }

    public void Kill()
    {
        score.Add(pointsWhenKilled);
        DropItem();
        Destroy(gameObject);
    }

    // Attempt to drop something.
    private void DropItem()
    {
        ItemType it = probItem.Roll();
        switch (it)
        {
            case ItemType.HealthKit:
                GameObject pup = Instantiate(prefabHealthKit, transform.position, Quaternion.identity);
                ItemHealthKit hk = pup.GetComponent<ItemHealthKit>();
                hk.SetHeal(healthPerHealthKit);
                hk.SetPointsPerFullHealthHealthKit(pointsPerFullHealthHealthKit);
                hk.SetScore(score);
                LeftMovement lm = pup.GetComponent<LeftMovement>();
                lm.SetMovementLeftSpeed(leftMovement.GetMovementLeftSpeed());
                break;
            case ItemType.BattleAxe:
                if (!playerHealth.GetPowerupActive())
                {
                    Instantiate(prefabBattleAxe, transform.position, Quaternion.identity);
                }
                break;
            case ItemType.MoreArms:
                if (!playerHealth.GetPowerupActive())
                {
                    Instantiate(prefabMoreArms, transform.position, Quaternion.identity);
                }
                break;
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