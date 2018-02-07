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

    public void Init(float angle, float speed)
    {
        SetAngleSpeed(angle, speed);
    }

    private void SetAngleSpeed(float angle, float speed)
    {
        angleSpeedMovement.SetAngleSpeed(angle, speed);
    }
}