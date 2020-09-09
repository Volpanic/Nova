﻿using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CameraController : MonoBehaviour
{
    public GameObject toFollow;
    private bool hasRigidBody = false;
    private Entity2D followBody = null;

    private Camera mainCam;

    public TilemapRenderer tmr;

    private Vector2 screenHalf = Vector2.zero;

    // Start is called before the first frame update
    void Start()
    {
        mainCam = GetComponent<Camera>();
        followBody = toFollow.GetComponent<Entity2D>();
        if(followBody != null)
        {
            hasRigidBody = true;
        }

        //Get Screen size
        screenHalf.y = mainCam.orthographicSize * 2.0f;
        screenHalf.x = screenHalf.y * mainCam.aspect;

        screenHalf *= 0.5f;

        screenHalf.x += 24.0f * Physics2DExtra.PIXEL_UNIT;
        screenHalf.y += 24.0f * Physics2DExtra.PIXEL_UNIT;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 target = transform.position;
        
        if (hasRigidBody)
        {
            target = new Vector3(toFollow.transform.position.x,
                toFollow.transform.position.y,
                transform.position.z);
        }
        else
        {
            target = new Vector3(toFollow.transform.position.x,
                toFollow.transform.position.y,
                transform.position.z);
        }

        transform.position = Vector3.Lerp(transform.position,target,0.32f);

        float hWidth = mainCam.orthographicSize;
        float hHeight = mainCam.orthographicSize;

        transform.position = new Vector3(Mathf.Clamp(transform.position.x,tmr.bounds.min.x + screenHalf.x,tmr.bounds.max.x - screenHalf.x),
            Mathf.Clamp(transform.position.y, tmr.bounds.min.y + screenHalf.y, tmr.bounds.max.y - screenHalf.y),
            transform.position.z);
    }
}
