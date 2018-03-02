// Author(s): Paul Calande
// Enemy projectile class.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    [System.Serializable]
    public class Data : IDeepCopyable<Data>
    {
        [Tooltip("Whether the projectile is punchable or not.")]
        public bool punchable;
        [Tooltip("How much damage the projectile does.")]
        public int damage;
        [Tooltip("How many points the projectile gives when punched.")]
        public int pointsWhenPunched;
        [Tooltip("The color of the projectile.")]
        public Color color;
        [Tooltip("The angle (direction) of the projectile.")]
        public float angle;
        [Tooltip("The speed of the projectile.")]
        public float speed;
        [Tooltip("Reference to the Score instance.")]
        public Score score;

        public Data(bool punchable, int damage, int pointsWhenPunched, Color color,
            float angle, float speed, Score score)
        {
            this.punchable = punchable;
            this.damage = damage;
            this.pointsWhenPunched = pointsWhenPunched;
            this.color = color;
            this.angle = angle;
            this.speed = speed;
            this.score = score;
        }

        public Data DeepCopy()
        {
            return new Data(punchable, damage, pointsWhenPunched, color,
                angle, speed, score);
        }
    }
    [SerializeField]
    Data data;

    [SerializeField]
    [Tooltip("Reference to the Velocity2D component.")]
    Velocity2D velocity;
    [SerializeField]
    [Tooltip("Reference to the SpriteRenderer component.")]
    SpriteRenderer spriteRenderer;

    public void SetData(Data val)
    {
        data = val;
    }

    private void Start()
    {
        SetAngleSpeed(data.angle, data.speed);
        SetColor(data.color);
    }

    public void SetAngleSpeed(float angleDegrees, float speed)
    {
        data.angle = angleDegrees;
        data.speed = speed;
        velocity.SetVelocity(UtilHeading2D.HeadingVectorFromDegrees(angleDegrees) * speed);
    }

    public void SetColor(Color val)
    {
        data.color = val;
        spriteRenderer.color = val;
    }

    // Kills the projectile, destroying it.
    public void Kill()
    {
        Destroy(gameObject);
    }

    private void PunchedByPlayer()
    {
        data.score.Add(data.pointsWhenPunched);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerPunch"))
        {
            if (data.punchable)
            {
                PunchedByPlayer();
                Kill();
            }
        }
        else if (collision.CompareTag("PlayerSelfHitbox"))
        {
            collision.transform.root.GetComponent<PlayerHealth>().Damage(data.damage);
            Kill();
        }
    }
}