// Author(s): Paul Calande
// Utility class for instantiating GameObjects in certain ways.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UtilInstantiate
{
    /// <summary>
    /// Instantiate a group of GameObjects that shall spread out at a given angle and direction.
    /// </summary>
    /// <param name="obj">The GameObject to use for instantiation.</param>
    /// <param name="position">The position at which to instantiate all the GameObjects.</param>
    /// <param name="count">The number of GameObjects to instantiate.</param>
    /// <param name="spreadAngle">The angle (in degrees) across which the GameObjects will spread.</param>
    /// <param name="direction">The direction (in degrees) marking the center of the spread angle.</param>
    /// <param name="rotateObjects">Whether to rotate the objects based on the spread direction.</param>
    public static DirectionObject[] SpreadAngleGroup(GameObject obj, Vector3 position, int count,
        float spreadAngle, float direction, bool rotateObjects = false)
    {
        List<DirectionObject> objects = new List<DirectionObject>();

        // The number of degrees between each GameObject's angle.
        float degreeDifference = spreadAngle / count;

        // The current angle to use for the current GameObject.
        float degreesSoFar = direction + (degreeDifference - spreadAngle) * 0.5f;

        for (int i = 0; i < count; ++i)
        {
            Quaternion rotation = Quaternion.identity;
            // Calculate the GameObject's rotation based on the current angle, if applicable.
            if (rotateObjects)
            {
                rotation = Quaternion.AngleAxis(degreesSoFar, Vector3.forward);
            }

            // Instantiate the GameObject and use it for the DirectionObject.
            GameObject newObj = GameObject.Instantiate(obj, position, rotation);
            DirectionObject directionObj = new DirectionObject(newObj, degreesSoFar);

            // Add the DirectionObject to the collection.
            objects.Add(directionObj);

            // Prepare for the next angle.
            degreesSoFar += degreeDifference;
        }

        return objects.ToArray();
    }
}