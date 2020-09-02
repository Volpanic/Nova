using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public static class Physics2DExtra
    {
        public const float PIXEL_SIZE = 16.0f;
        public const float PIXEL_UNIT = 1.0f / PIXEL_SIZE;

        public static bool PlaceMeeting(ref BoxCollider2D collider, Vector2 offset, int layerMask)
        {
            Vector2 topLeft = new Vector2(collider.bounds.min.x + PIXEL_UNIT, collider.bounds.max.y - PIXEL_UNIT);
            Vector2 botRight = new Vector2(collider.bounds.max.x - PIXEL_UNIT, collider.bounds.min.y + PIXEL_UNIT);

            var result = Physics2D.OverlapArea(topLeft + offset, botRight + offset, layerMask);
            if(result != null && !result.isTrigger)
            {
                return true;
            }
            return false;
        }
        
        public static bool PlaceMeeting(ref Collider2D collider, Vector2 offset, int layerMask , ref Collider2D otherCollider, bool includeIsTrigger = false)
        {
            Vector2 topLeft = new Vector2(collider.bounds.min.x + PIXEL_UNIT, collider.bounds.max.y - PIXEL_UNIT);
            Vector2 botRight = new Vector2(collider.bounds.max.x - PIXEL_UNIT, collider.bounds.min.y + PIXEL_UNIT);

            var result = Physics2D.OverlapArea(topLeft + offset, botRight + offset, layerMask);
            if (result != null && (!result.isTrigger || includeIsTrigger) && result == otherCollider)
            {
                return true;
            }
            return false;
        }

        public static bool PlaceMeeting(Collider2D collider, Vector2 offset, int layerMask, Collider2D otherCollider, bool includeIsTrigger = false)
        {
            Vector2 topLeft = new Vector2(collider.bounds.min.x + PIXEL_UNIT, collider.bounds.max.y - PIXEL_UNIT);
            Vector2 botRight = new Vector2(collider.bounds.max.x - PIXEL_UNIT, collider.bounds.min.y + PIXEL_UNIT);

            var result = Physics2D.OverlapArea(topLeft + offset, botRight + offset, layerMask);
            if (result != null && (!result.isTrigger || includeIsTrigger) && result == otherCollider)
            {
                return true;
            }
            return false;
        }

        public static float Right(Collider2D collider)
        {
            return collider.bounds.max.x - PIXEL_UNIT;
        }
        public static float Left(Collider2D collider)
        {
            return collider.bounds.min.x + PIXEL_UNIT;
        }
        public static float Top(Collider2D collider)
        {
            return collider.bounds.max.y - PIXEL_UNIT;
        }
        public static float Bottom(Collider2D collider)
        {
            return collider.bounds.min.y + PIXEL_UNIT;
        }
    }
}
