// Author(s): Paul Calande
// Health kit item for Rocket Puncher.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHealthKit : MonoBehaviour
{
    [System.Serializable]
    public class Data
    {
        [Tooltip("How much the health kit heals.")]
        public int heal;
        [Tooltip("How many points the health kit gives when the player is at full health.")]
        public int pointsPerFullHealthHealthKit;
        [Tooltip("Reference to the Score instance.")]
        public Score score;

        public Data(int heal, int pointsPerFullHealthHealthKit,
            Score score)
        {
            this.heal = heal;
            this.pointsPerFullHealthHealthKit = pointsPerFullHealthHealthKit;
            this.score = score;
        }
    }
    [SerializeField]
    [Tooltip("Health kit data.")]
    Data data;

    public void SetData(Data val)
    {
        data = val;
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
                // Award points.
                data.score.Add(data.pointsPerFullHealthHealthKit);
            }
            else
            {
                playerHealth.Heal(data.heal);
            }
            Destroy(gameObject);
        }
    }
}