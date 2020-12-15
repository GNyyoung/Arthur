using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class GUIBoundary : MonoBehaviour
{
    public float width;
    public float height;
    

    private void OnDrawGizmos()
    {
        var startPos = transform.position + (new Vector3(-width, height)) / 2.0f;
        var endPos = transform.position + (new Vector3(width, -height) / 2.0f);
        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(new Vector3(startPos.x, startPos.y), new Vector3(endPos.x, startPos.y));
        Gizmos.DrawLine(new Vector3(endPos.x, startPos.y), new Vector3(endPos.x, endPos.y));
        Gizmos.DrawLine(new Vector3(endPos.x, endPos.y), new Vector3(startPos.x, endPos.y));
        Gizmos.DrawLine(new Vector3(startPos.x, endPos.y), new Vector3(startPos.x, startPos.y));
    }
}
