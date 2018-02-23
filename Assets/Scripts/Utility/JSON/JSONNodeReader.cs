// Author(s): Paul Calande
// Wrapper class for a JSON node, with some additional convenient methods.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

public class JSONNodeReader
{
    // The node to read from.
    JSONNode myNode;

    // Construct based on a file reference, making its root the node to be read from.
    public JSONNodeReader(TextAsset file)
    {
        SetFile(file);
    }

    // Construct based on a given node.
    public JSONNodeReader(JSONNode node)
    {
        myNode = node;
    }

    // Change the file being read.
    public void SetFile(TextAsset file)
    {
        myNode = JSON.Parse(file.ToString());
    }

    // Attempt to read a JSON node. If the node does not exist, return the default value.
    public int TryGetInt(string nodeName, int defaultValue)
    {
        JSONNode node = myNode[nodeName];
        if (node == null)
        {
            return defaultValue;
        }
        else
        {
            return node.AsInt;
        }
    }

    public float TryGetFloat(string nodeName, float defaultValue)
    {
        JSONNode node = myNode[nodeName];
        if (node == null)
        {
            return defaultValue;
        }
        else
        {
            return node.AsFloat;
        }
    }

    public bool TryGetBool(string nodeName, bool defaultValue)
    {
        JSONNode node = myNode[nodeName];
        if (node == null)
        {
            return defaultValue;
        }
        else
        {
            return node.AsBool;
        }
    }

    public string TryGetString(string nodeName, string defaultValue)
    {
        JSONNode node = myNode[nodeName];
        if (node == null)
        {
            return defaultValue;
        }
        else
        {
            return node;
        }
    }

    // Tries to get a JSON object with the given node name.
    // Returns null if no object with the given name is found.
    public JSONNodeReader TryGetNode(string nodeName)
    {
        JSONNode node = myNode[nodeName];
        if (node == null)
        {
            return null;
        }
        else
        {
            return new JSONNodeReader(node);
        }
    }

    // Returns null if no array with the given name is found.
    public JSONArrayReader TryGetArray(string nodeName)
    {
        JSONNode node = myNode[nodeName];
        if (node == null)
        {
            return null;
        }
        else
        {
            return new JSONArrayReader(node.AsArray);
        }
    }
}