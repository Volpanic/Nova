using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public static class Physics2DExtra
    {
        public static bool PlaceMeeting(ref BoxCollider2D collider, Vector2 offset, int layerMask)
        {
            return Physics2D.OverlapArea(collider.bounds.min + (Vector3)offset, collider.bounds.max + (Vector3)offset, layerMask);
        }
    }
}
