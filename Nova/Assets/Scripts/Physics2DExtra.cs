using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public static class Physics2DExtra
    {
        const float PIXEL_SIZE = 16.0f;
        const float PIXEL_UNIT = 1.0f / PIXEL_SIZE;

        public static bool PlaceMeeting(ref BoxCollider2D collider, Vector2 offset, int layerMask)
        {
            Vector2 topLeft = new Vector2(collider.bounds.min.x + PIXEL_UNIT, collider.bounds.max.y - PIXEL_UNIT);
            Vector2 botRight = new Vector2(collider.bounds.max.x - PIXEL_UNIT, collider.bounds.min.y + PIXEL_UNIT);
            return Physics2D.OverlapArea(topLeft + offset, botRight + offset, layerMask);
        }
    }
}
