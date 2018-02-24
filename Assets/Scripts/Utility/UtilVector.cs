// Author(s): Paul Calande
// Vector-related utility functions.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UtilVector : MonoBehaviour
{
    // Returns the signed angle (-180 to 180 degrees) between two points.
    public static float GetSignedDirectionToPoint(Vector3 startPosition, Vector3 endPosition)
    {
        return Vector3.SignedAngle(Vector3.right,
            endPosition - startPosition, Vector3.forward);
    }
}