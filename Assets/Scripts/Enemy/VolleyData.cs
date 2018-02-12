// Author(s): Paul Calande
// Data for a volley.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class VolleyData
{
    [SerializeField]
    [Tooltip("The speed of fired volleys.")]
    public float speed;
    [SerializeField]
    [Tooltip("The (initial) direction of fired volleys.")]
    public float direction;
    [SerializeField]
    [Tooltip("How many projectiles are spawned per volley.")]
    public int projectileCount;
    [SerializeField]
    [Tooltip("The spread of projectiles (in degrees) across one volley.")]
    public float spreadAngle;
    [SerializeField]
    [Tooltip("Whether the fired projectiles are punchable.")]
    public bool projectilePunchable;
    [SerializeField]
    [Tooltip("Whether the volley is aimed at the player.")]
    public bool aimAtPlayer;
    [SerializeField]
    [Tooltip("The color of the volley.")]
    public Color color;
}