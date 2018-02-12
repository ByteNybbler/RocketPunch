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
    [Tooltip("How many seconds should pass between each volley.")]
    float secondsBetweenVolleys;
    [SerializeField]
    [Tooltip("The speed of fired volleys.")]
    float volleySpeed;
    [SerializeField]
    [Tooltip("The (initial) direction of fired volleys.")]
    float volleyDirection;
    [SerializeField]
    [Tooltip("The change in volley direction between each shot.")]
    float volleyDirectionDeltaPerShot;
    [SerializeField]
    [Tooltip("How many projectiles are spawned per volley.")]
    int volleyProjectileCount;
    [SerializeField]
    [Tooltip("The spread of projectiles (in degrees) across one volley.")]
    float volleySpreadAngle;
    [SerializeField]
    [Tooltip("Whether the fired projectiles are punchable.")]
    bool projectilePunchable;
    [SerializeField]
    [Tooltip("Whether the enemy aims its volleys at the player.")]
    bool volleyAimAtPlayer;

    GameObject player;
    Timer timerVolley;

    private void Start()
    {
        player = ServiceLocator.GetPlayer();
        timerVolley = new Timer(secondsBetweenVolleys);
    }

    private void FixedUpdate()
    {
        while (timerVolley.TimeUp(Time.deltaTime))
        {
            float[] angles = UtilSpread.PopulateAngle(volleySpreadAngle, volleyDirection, volleyProjectileCount);
            foreach (float a in angles)
            {
                float angle = a;
                if (volleyAimAtPlayer)
                {
                    Vector3 playerPos = player.transform.position;
                    angle += Vector3.Angle(Vector3.left, transform.position - playerPos);
                }
                Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                GameObject projectile = Instantiate(prefabProjectile, transform.position, rotation);
                EnemyProjectile proj = projectile.GetComponent<EnemyProjectile>();
                proj.SetAngleSpeed(angle, volleySpeed);
                proj.SetPunchable(projectilePunchable);
            }
            volleyDirection += volleyDirectionDeltaPerShot;
        }
    }
}