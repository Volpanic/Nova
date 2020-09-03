using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EntityFollowAlongPath))]
public class EntityFollowAlongPathEditor : Editor
{
    EntityFollowAlongPath entityPath;
    GUIStyle uiStyle = new GUIStyle();

    private void OnEnable()
    {
        entityPath = target as EntityFollowAlongPath;
        uiStyle.alignment = TextAnchor.MiddleCenter;
        uiStyle.fontStyle = FontStyle.Bold;

        if(entityPath.pointPositions.Count <= 0)
        {
            entityPath.pointPositions.Add(entityPath.transform.position);
        }
    }

    private void OnSceneGUI()
    {
        for(int i  = 0; i < entityPath.pointPositions.Count; i++)
        {
            Handles.color = Color.white;

            if (i != 0)
            {
                entityPath.pointPositions[i] = ((Vector2)Handles.FreeMoveHandle(entityPath.pointPositions[i], Quaternion.identity, .25f, Vector3.zero, Handles.RectangleHandleCap));
                entityPath.pointPositions[i] = new Vector2((Mathf.Round(entityPath.pointPositions[i].x * 1f) / 1f),
                    (Mathf.Round(entityPath.pointPositions[i].y * 1f) /1f));
            }

            Handles.DrawSolidDisc(entityPath.pointPositions[i],Vector3.forward,.25f);
            
            if(i != entityPath.pointPositions.Count-1)
            {
                Handles.DrawDottedLine(entityPath.pointPositions[i], entityPath.pointPositions[i+1],8);   
            }
            else if(i > 0)
            {
                if(entityPath.pathType == EntityFollowAlongPath.PathType.Closed)
                { 
                    Handles.DrawDottedLine(entityPath.pointPositions[0], entityPath.pointPositions[i], 8);
                    break;
                }
                
            }

            Event guiEvent = Event.current;


                Ray mouseRay = HandleUtility.GUIPointToWorldRay(guiEvent.mousePosition);
                float drawPlaneDepth = 0;
                float dstToDrawPlane = (drawPlaneDepth - mouseRay.origin.y) / mouseRay.direction.y;
                Vector2 worldPos = mouseRay.GetPoint(dstToDrawPlane);

            Handles.DrawSolidDisc(worldPos,Vector3.forward,2);

               // entityPath.pointPositions.Add(worldPos);
            

            Handles.color = Color.black;
            Handles.Label(entityPath.pointPositions[i], i.ToString(), uiStyle);
        }
    }
}
