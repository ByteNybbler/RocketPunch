// Author(s): Paul Calande
// Script for enemy attacks.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Prefab to use for the projectile.")]
    GameObject prefabProjectile;
    [SerializeField]
    [Tooltip("How many seconds occur between each projectile shot.")]
    float secondsBetweenShots;
    [SerializeField]
    [Tooltip("The speed of fired projectiles.")]
    float projectileSpeed;
    [SerializeField]
    [Tooltip("The angle of fired projectiles.")]
    float projectileAngle;
    [SerializeField]
    [Tooltip("The change in projectile angle between each shot.")]
    float projectileAngleDeltaPerShot;
    [SerializeField]
    [Tooltip("Whether the fired projectiles are punchable.")]
    bool projectilePunchable;

    Timer timerShot;

    private void Start()
    {
        timerShot = new Timer(secondsBetweenShots);
    }

    private void FixedUpdate()
    {
        while (timerShot.TimeUp(Time.deltaTime))
        {
            GameObject projectile = Instantiate(prefabProjectile, transform.position, Quaternion.identity);
            EnemyProjectile proj = projectile.GetComponent<EnemyProjectile>();
            proj.SetAngleSpeed(projectileAngle, projectileSpeed);
            proj.SetPunchable(projectilePunchable);
            projectileAngle += projectileAngleDeltaPerShot;
        }
    }
}