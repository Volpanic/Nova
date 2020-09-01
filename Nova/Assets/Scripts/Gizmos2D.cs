using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Assets.Scripts
{
    public static class Gizmos2D
    {
        public static Color color { set { Gizmos.color = value; }}

        public static void DrawRectangle(Vector2 topLeft, Vector2 bottemRight)
        {
            Gizmos.DrawLine(topLeft, new Vector2(topLeft.x,bottemRight.y));
            Gizmos.DrawLine(topLeft, new Vector2(bottemRight.x,topLeft.y)); 
            
            Gizmos.DrawLine(bottemRight, new Vector2(bottemRight.x, topLeft.y));
            Gizmos.DrawLine(bottemRight, new Vector2(topLeft.x, bottemRight.y));
        }

    }
}
