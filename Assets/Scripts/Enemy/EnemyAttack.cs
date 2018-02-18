// Author(s): Paul Calande
// Script for enemy attacks.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [System.Serializable]
    public class Data
    {
        [Tooltip("The volley that the enemy should fire.")]
        public VolleyData volley;
        [Tooltip("How many seconds should pass between each volley.")]
        public float secondsBetweenVolleys;
        [Tooltip("The change in volley direction between each shot.")]
        public float volleyDirectionDeltaPerShot;
        [Tooltip("The left speed that will be applied to fired projectiles.")]
        public float projectileLeftSpeed;

        /*
        [Tooltip("How many points each projectile gives when punched.")]
        public int pointsPerProjectilePunched;
        [Tooltip("Reference to the Score instance.")]
        public Score score;
        */

        public Data(VolleyData volley,
            float secondsBetweenVolleys,
            float volleyDirectionDeltaPerShot,
            float projectileLeftSpeed)
        {
            this.volley = volley;
            this.secondsBetweenVolleys = secondsBetweenVolleys;
            this.volleyDirectionDeltaPerShot = volleyDirectionDeltaPerShot;
            this.projectileLeftSpeed = projectileLeftSpeed;
        }
    }
    [SerializeField]
    Data data;

    [SerializeField]
    [Tooltip("Prefab to use for the projectile.")]
    GameObject prefabProjectile;

    // Reference to the player object.
    GameObject player;
    // Timer for firing volleys.
    Timer timerVolley;

    public void SetData(Data val)
    {
        data = val;
    }

    private void Start()
    {
        player = ServiceLocator.GetPlayer();
        timerVolley = new Timer(data.secondsBetweenVolleys);
    }

    private void FixedUpdate()
    {
        while (timerVolley.TimeUp(Time.deltaTime))
        {
            VolleyData volley = data.volley;
            float[] angles = UtilSpread.PopulateAngle(volley.spreadAngle,
                data.volley.projectile.angle,
                volley.projectileCount);
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
                EnemyProjectile.Data projData = data.volley.projectile.DeepCopy();
                projData.angle = angle;
                proj.SetData(projData);
                LeftMovement lm = projectile.GetComponent<LeftMovement>();
                lm.SetMovementLeftSpeed(data.projectileLeftSpeed);
            }
            data.volley.projectile.angle += data.volleyDirectionDeltaPerShot;
        }
    }
}