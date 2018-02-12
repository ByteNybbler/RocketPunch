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
}