// Author(s): Paul Calande
// Script for managing an enemy's sprite.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySprite : MonoBehaviour
{
    [System.Serializable]
    public class Data : IDeepCopyable<Data>
    {
        [Tooltip("The name of the sprite to use.")]
        public string spriteName;

        public Data (string spriteName)
        {
            this.spriteName = spriteName;
        }

        public Data DeepCopy()
        {
            return new Data(spriteName);
        }
    }
    [SerializeField]
    Data data;

    [System.Serializable]
    class SpriteDictEntry
    {
        public string name = null;
        public Sprite sprite = null;
    }

    [SerializeField]
    [Tooltip("Dictionary of the values to use.")]
    SpriteDictEntry[] spriteDict;
    [SerializeField]
    [Tooltip("Reference to the renderer to use.")]
    SpriteRenderer render;

    public void SetData(Data val)
    {
        data = val;
    }

    private void Start()
    {
        foreach (SpriteDictEntry sde in spriteDict)
        {
            if (sde.name == data.spriteName)
            {
                render.sprite = sde.sprite;
                break;
            }
        }
    }
}