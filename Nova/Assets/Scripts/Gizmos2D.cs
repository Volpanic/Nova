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

        public static void DrawCircle(Vector2 center, float radius, int persicison = 8)
        {
            float inc = (Mathf.PI * 2.0f) / persicison;
            float cur = 0;

            Vector2 oldPos = new Vector2(Mathf.Cos(cur), Mathf.Sin(cur)) * radius;

            for(int i = 1; i <= persicison; i++)
            {
                cur += inc;
                Vector2 newPos = new Vector2(Mathf.Cos(cur), Mathf.Sin(cur)) * radius;
                Gizmos.DrawLine(center + oldPos, center + newPos);

                oldPos = newPos;
            }
        }

    }
}
