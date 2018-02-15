// Author(s): Paul Calande
// Class for returning one result from a set by random chance.
// T is the result type.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Probability<T>
{
    // The dictionary of results and their associated chances.
    Dictionary<T, float> chances = new Dictionary<T, float>();
    // The default result.
    // This is the result returned when no other result is chosen.
    T resultDefault;

    public Probability(T resultDefault)
    {
        this.resultDefault = resultDefault;
    }

    public void SetDefaultResult(T val)
    {
        resultDefault = val;
    }

    // Sets the chance of the given result occurring.
    public void SetChance(T result, float chance)
    {
        chances[result] = chance;
    }

    // Removes the chance of the given result occurring.
    public void RemoveChance(T result)
    {
        chances.Remove(result);
    }

    // Returns a random result. If no result is chosen, the default result is returned,
    // which is possible as long as the combined chances don't add up to 1.
    public T Roll()
    {
        float roll = Random.Range(0.0f, 1.0f);
        float cumulativeChance = 0.0f;
        foreach (var pair in chances)
        {
            T result = pair.Key;
            float chance = pair.Value;
            cumulativeChance += chance;
            if (roll <= cumulativeChance)
            {
                return result;
            }
        }
        return resultDefault;
    }
}