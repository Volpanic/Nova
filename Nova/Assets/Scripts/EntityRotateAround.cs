using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityRotateAround : MonoBehaviour
{
    public float Radius = 1;

    private Vector3 initPos;

    // Start is called before the first frame update
    void Start()
    {
        initPos = transform.position;
    }

    private void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            Gizmos2D.DrawCircle(initPos,Radius);
        }
        else
        {
            Gizmos2D.DrawCircle(transform.position, Radius);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

