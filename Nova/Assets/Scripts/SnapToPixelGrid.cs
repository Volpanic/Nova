using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapToPixelGrid : MonoBehaviour // Script by Ryan Nielson (https://nielson.dev/2015/08/the-pixel-grid-better-2d-in-unity-part-1)
{
    public int pixelsPerUnit = 16;

    private void Start()
    {
         
    }

    private void LateUpdate()
    {
        Vector3 newLocalPosition = Vector3.zero;
        newLocalPosition.z = transform.position.z;

        newLocalPosition.x = (Mathf.Round(transform.position.x * pixelsPerUnit) / pixelsPerUnit);
        newLocalPosition.y = (Mathf.Round(transform.position.y * pixelsPerUnit) / pixelsPerUnit);

        transform.position = newLocalPosition;
    }
}
