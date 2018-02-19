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
    public EnemySprite.Data spriteData;

    public EnemyData(float challenge, float leftMovementSpeedBonus,
        OscillatePosition2D.Data oscData, EnemyAttack.Data attackData,
        EnemySprite.Data spriteData)
    {
        this.challenge = challenge;
        this.leftMovementSpeedBonus = leftMovementSpeedBonus;
        this.oscData = oscData;
        this.attackData = attackData;
        this.spriteData = spriteData;
    }
}