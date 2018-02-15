// Author(s): Paul Calande
// Health kit item for Rocket Puncher.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHealthKit : MonoBehaviour
{
    [SerializeField]
    [Tooltip("How much the health kit heals.")]
    int heal;
    [SerializeField]
    [Tooltip("How many points the health kit gives when the player is at full health.")]
    int pointsPerFullHealthHealthKit;
    [SerializeField]
    [Tooltip("Reference to the Score instance.")]
    Score score;

    public void SetHeal(int val)
    {
        heal = val;
    }
    public void SetPointsPerFullHealthHealthKit(int val)
    {
        pointsPerFullHealthHealthKit = val;
    }
    public void SetScore(Score val)
    {
        score = val;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerSelfHitbox"))
        {
            Transform root = collision.transform.root;
            PlayerHealth playerHealth = root.GetComponent<PlayerHealth>();
            Health health = root.GetComponent<Health>();
            if (health.IsHealthFull())
            {
                // Reward points.
                score.Add(pointsPerFullHealthHealthKit);
            }
            else
            {
                playerHealth.Heal(heal);
            }
            Destroy(gameObject);
        }
    }
}