// Author(s): Paul Calande
// Script for tiling.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileRenderer : MonoBehaviour
{
    [SerializeField]
    SpriteRenderer render;
    /*
    [SerializeField]
    GameObject child;
    */

    [SerializeField]
    LinearModulo2D lm;
    [SerializeField]
    float leftMovementSpeed;

    float width;

    private void Start()
    {
        width = render.bounds.size.x;
        //Debug.Log(width);

        Vector3 offset = new Vector3(width, 0.0f, 0.0f);

        //Instantiate(gameObject, transform.position + offset, Quaternion.identity);
        GameObject child = new GameObject();
        child.transform.parent = transform;
        child.transform.position = transform.position + offset;
        child.transform.localScale = Vector3.one;
        SpriteRenderer sr = child.AddComponent<SpriteRenderer>();
        sr.sprite = render.sprite;
        sr.sortingLayerID = render.sortingLayerID;
        sr.sortingOrder = render.sortingOrder;
        //child.transform.position += offset;

        //a.drawMode = SpriteDrawMode.Tiled;

        LinearModulo2D.Data lmData = new LinearModulo2D.Data(
            new Vector2(-leftMovementSpeed, 0.0f),
            width);
        lm.SetData(lmData);
    }

    public float GetTileWidth()
    {
        return width;
    }
}