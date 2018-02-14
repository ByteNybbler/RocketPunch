// Author(s): Paul Calande
// really...

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileRenderer : MonoBehaviour
{
    [SerializeField]
    Renderer render;

    private void Start()
    {
        float width = render.bounds.size.x;
        Debug.Log(width);

        Vector3 offset = new Vector3(width, 0.0f, 0.0f);

        Instantiate(gameObject, transform.position + offset, Quaternion.identity);

        //a.drawMode = SpriteDrawMode.Tiled;
    }
}