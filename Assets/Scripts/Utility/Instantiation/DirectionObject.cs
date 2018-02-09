// Author(s): Paul Calande
// Class representing a GameObject paired with a direction.

using UnityEngine;

public class DirectionObject
{
    GameObject obj;
    float direction;

    public DirectionObject(GameObject obj, float direction)
    {
        this.obj = obj;
        this.direction = direction;
    }

    public GameObject GetGameObject()
    {
        return obj;
    }

    public float GetDirection()
    {
        return direction;
    }
}