// Author(s): Paul Calande
// Utility class for JSON functionality.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

public class UtilJSON
{
    // Attempt to read a JSON node. If it does not exist, return the default value.
    public static int TryReadInt(JSONNode node, int defaultValue)
    {
        if (node == null)
        {
            return defaultValue;
        }
        else
        {
            return node.AsInt;
        }
    }
    public static float TryReadFloat(JSONNode node, float defaultValue)
    {
        if (node == null)
        {
            return defaultValue;
        }
        else
        {
            return node.AsFloat;
        }
    }
    public static bool TryReadBool(JSONNode node, bool defaultValue)
    {
        if (node == null)
        {
            return defaultValue;
        }
        else
        {
            return node.AsBool;
        }
    }
    public static string TryReadString(JSONNode node, string defaultValue)
    {
        if (node == null)
        {
            return defaultValue;
        }
        else
        {
            return node;
        }
    }
}