// Author(s): Paul Calande
// Class containing data that represents a Rocket Puncher enemy.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyData
{
    float challenge;
    public float leftMovementSpeed;
    public float yOscillationMagnitude;
    public float yOscillationSpeed;
    public VolleyData volley;
    public float secondsBetweenVolleys;
    public float volleyDirectionDeltaPerShot;

    public EnemyData(float challenge)
    {
        this.challenge = challenge;
    }

    public float GetChallenge()
    {
        return challenge;
    }
}