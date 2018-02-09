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

    Timer timerVolley;

    private void Start()
    {
        timerVolley = new Timer(secondsBetweenVolleys);
    }

    private void FixedUpdate()
    {
        while (timerVolley.TimeUp(Time.deltaTime))
        {
            DirectionObject[] objects = UtilInstantiate.SpreadAngleGroup(prefabProjectile,
                transform.position, volleyProjectileCount, volleySpreadAngle,
                volleyDirection);
            //GameObject projectile = Instantiate(prefabProjectile, transform.position, Quaternion.identity);

            foreach (DirectionObject obj in objects)
            {
                GameObject projectile = obj.GetGameObject();
                float direction = obj.GetDirection();
                EnemyProjectile proj = projectile.GetComponent<EnemyProjectile>();
                proj.SetAngleSpeed(direction, volleySpeed);
                proj.SetPunchable(projectilePunchable);
            }
            volleyDirection += volleyDirectionDeltaPerShot;
        }
    }
}