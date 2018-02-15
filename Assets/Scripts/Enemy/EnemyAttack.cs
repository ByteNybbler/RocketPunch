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
    [SerializeField]
    [Tooltip("The left speed that will be applied to fired projectiles.")]
    float projectileLeftSpeed;
    [SerializeField]
    [Tooltip("How many points each projectile gives when punched.")]
    int pointsPerProjectilePunched;
    [SerializeField]
    [Tooltip("Reference to the Score instance.")]
    Score score;

    // Reference to the player object.
    GameObject player;
    // Timer for firing volleys.
    Timer timerVolley;

    public void Init(VolleyData volley, float secondsBetweenVolleys, float volleyDirectionDeltaPerShot,
        float projectileLeftSpeed, Score score)
    {
        this.volley = volley;
        this.secondsBetweenVolleys = secondsBetweenVolleys;
        this.volleyDirectionDeltaPerShot = volleyDirectionDeltaPerShot;
        this.projectileLeftSpeed = projectileLeftSpeed;
        this.score = score;
        this.pointsPerProjectilePunched = score.GetPointsPerProjectilePunched();
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
                proj.SetColor(volley.color);
                proj.SetScore(score);
                proj.SetPoints(pointsPerProjectilePunched);
                LeftMovement lm = projectile.GetComponent<LeftMovement>();
                lm.SetMovementLeftSpeed(projectileLeftSpeed);
            }
            volley.direction += volleyDirectionDeltaPerShot;
        }
    }
}