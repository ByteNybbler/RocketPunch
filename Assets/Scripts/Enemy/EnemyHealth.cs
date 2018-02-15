// Author(s): Paul Calande
// Enemy health class for Rocket Puncher.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Reference to the Score instance.")]
    Score score;
    [SerializeField]
    [Tooltip("How many points the enemy gives when it's killed.")]
    int pointsWhenKilled;
    [SerializeField]
    [Tooltip("How many points the dropped health kit gives when the player is at full health.")]
    int pointsPerFullHealthHealthKit;

    public void SetScore(Score val)
    {
        score = val;
    }
    public void SetPointsWhenKilled(int val)
    {
        pointsWhenKilled = val;
    }
    public void SetPointsPerFullHealthHealthKit(int val)
    {
        pointsPerFullHealthHealthKit = val;
    }

    public void Kill()
    {
        score.Add(pointsWhenKilled);
        Destroy(gameObject);

        // Attempt to drop something.
        // TODO
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerPunch"))
        {
            Kill();
        }
    }
}