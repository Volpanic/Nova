﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR

using UnityEditor;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(EntityFollowAlongPath))]
public class EntityFollowAlongPathEditor : Editor
{
    EntityFollowAlongPath entityPath;
    GUIStyle uiStyle = new GUIStyle();

    public int selectedNode = 0;

    private void OnEnable()
    {
        entityPath = target as EntityFollowAlongPath;
        uiStyle.alignment = TextAnchor.MiddleCenter;
        uiStyle.fontStyle = FontStyle.Bold;

        if(entityPath.pointPositions.Count == 0)
        {
           entityPath.pointPositions.Add(entityPath.transform.position);
        }
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Add Point"))
        {
            int pointCount = entityPath.pointPositions.Count;
            if (pointCount <= 1)
            {
                entityPath.pointPositions.Add(entityPath.pointPositions[0] + new Vector2(0,1));
            }
            else
            {
                Vector2 normal = (entityPath.pointPositions[pointCount - 1] - entityPath.pointPositions[pointCount - 2]).normalized;
                entityPath.pointPositions.Add(entityPath.pointPositions[pointCount - 1] + normal);
            }
            SceneView.RepaintAll();
        }
       

        if (GUILayout.Button("Center Points"))
        {
            float inc = (Mathf.PI * 2.0f) / (entityPath.pointPositions.Count - 1);
            float cur = 0;

            Vector2 oldPos = new Vector2(Mathf.Cos(cur), Mathf.Sin(cur)) * entityPath.pointPositions.Count;

            for (int i = 0; i < entityPath.pointPositions.Count; i++)
            {
                if(i == 0)
                {
                    entityPath.pointPositions[i] = (entityPath.transform.position);
                }
                else
                {
                    cur += inc;
                    Vector2 newPos = new Vector2(Mathf.Cos(cur), Mathf.Sin(cur)) * entityPath.pointPositions.Count;
                    entityPath.pointPositions[i] = (Vector2)entityPath.transform.position + (newPos);

                    oldPos = newPos;
                }
            }
            SceneView.RepaintAll();
        }

        GUILayout.EndHorizontal();

        if (GUI.changed)
        {
            EditorUtility.SetDirty(entityPath);
            EditorSceneManager.MarkSceneDirty(entityPath.gameObject.scene);
        }
    }

    private void OnSceneGUI()
    {
        Event guiEvent = Event.current;

        for (int i  = 0; i < entityPath.pointPositions.Count; i++)
        {
            Handles.color = Color.white;

            if (i != 0)
            {
                if (!Application.isPlaying)
                {
                    entityPath.pointPositions[i] = ((Vector2)Handles.FreeMoveHandle(entityPath.pointPositions[i], Quaternion.identity, 0.25f, Vector3.zero, Handles.RectangleHandleCap));
                    entityPath.pointPositions[i] = new Vector2((Mathf.Floor(entityPath.pointPositions[i].x * 2f) / 2f),
                        (Mathf.Floor(entityPath.pointPositions[i].y * 2f) / 2f));
                }
            }
            else
            {
                if(!Application.isPlaying)
                {
                    entityPath.pointPositions[i] = (entityPath.transform.position);
                }
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

            Ray mouseRay = HandleUtility.GUIPointToWorldRay(guiEvent.mousePosition);
            float drawPlaneDepth = 0;
            float dstToDrawPlane = (drawPlaneDepth - mouseRay.origin.y) / mouseRay.direction.y;
            Vector2 worldPos = mouseRay.GetPoint(dstToDrawPlane);

            Handles.DrawSolidDisc(worldPos,Vector3.forward,2);

               // entityPath.pointPositions.Add(worldPos);
            

            Handles.color = Color.black;
            Handles.Label(entityPath.pointPositions[i], i.ToString(), uiStyle);
        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(entityPath);
            EditorSceneManager.MarkSceneDirty(entityPath.gameObject.scene);
        }
    }
}

#endif