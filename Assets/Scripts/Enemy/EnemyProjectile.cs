// Author(s): Paul Calande
// Enemy projectile class.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Reference to the AngleSpeedMovement component.")]
    AngleSpeedMovement2D angleSpeedMovement;
    [SerializeField]
    [Tooltip("Whether the projectile is punchable or not.")]
    bool punchable;

    public void SetAngleSpeed(float angle, float speed)
    {
        angleSpeedMovement.SetAngleSpeed(angle, speed);
    }

    public void SetPunchable(bool val)
    {
        punchable = val;
    }

    // Kills the projectile, destroying it.
    public void Kill()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerPunch"))
        {
            if (punchable)
            {
                Kill();
            }
        }
    }
}