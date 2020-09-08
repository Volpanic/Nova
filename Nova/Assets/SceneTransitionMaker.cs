using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTransitionMaker : MonoBehaviour
{
    public GameObject sceneTransitionCanvas = null;

    public void DoTransition(string roomName)
    {
        GameObject canvas = GameObject.Instantiate(sceneTransitionCanvas,Vector3.zero,Quaternion.identity);
        canvas.GetComponentInChildren<SceneTransition>().sceneToGoTo = roomName;
        canvas.GetComponentInChildren<SceneTransition>().BeginTransition();
    }
}
