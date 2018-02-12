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
    [Tooltip("The volley that the enemy should fire.")]
    VolleyData volley;
    [SerializeField]
    [Tooltip("How many seconds should pass between each volley.")]
    float secondsBetweenVolleys;
    [SerializeField]
    [Tooltip("The change in volley direction between each shot.")]
    float volleyDirectionDeltaPerShot;
    /*
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
    */

    GameObject player;
    Timer timerVolley;

    public void Init(VolleyData volley, float secondsBetweenVolleys, float volleyDirectionDeltaPerShot)
    {
        this.volley = volley;
        this.secondsBetweenVolleys = secondsBetweenVolleys;
        this.volleyDirectionDeltaPerShot = volleyDirectionDeltaPerShot;
    }

    private void Start()
    {
        player = ServiceLocator.GetPlayer();
        timerVolley = new Timer(secondsBetweenVolleys);
    }

    private void FixedUpdate()
    {
        while (timerVolley.TimeUp(Time.deltaTime))
        {
            float[] angles = UtilSpread.PopulateAngle(volley.spreadAngle, volley.direction, volley.projectileCount);
            foreach (float a in angles)
            {
                float angle = a;
                if (volley.aimAtPlayer)
                {
                    Vector3 playerPos = player.transform.position;
                    angle += Vector3.Angle(Vector3.left, transform.position - playerPos);
                }
                Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                GameObject projectile = Instantiate(prefabProjectile, transform.position, rotation);
                EnemyProjectile proj = projectile.GetComponent<EnemyProjectile>();
                proj.SetAngleSpeed(angle, volley.speed);
                proj.SetPunchable(volley.projectilePunchable);
            }
            volley.direction += volleyDirectionDeltaPerShot;
        }
    }
}