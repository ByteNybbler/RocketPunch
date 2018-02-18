// Author(s): Paul Calande
// Class containing data that represents a Rocket Puncher enemy.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyData
{
    public float challenge;
    public float leftMovementSpeedBonus;
    public OscillatePosition2D.Data oscData;
    public EnemyAttack.Data attackData;

    public EnemyData(float challenge, float leftMovementSpeedBonus,
        OscillatePosition2D.Data oscData, EnemyAttack.Data attackData)
    {
        this.challenge = challenge;
        this.leftMovementSpeedBonus = leftMovementSpeedBonus;
        this.oscData = oscData;
        this.attackData = attackData;
    }
}