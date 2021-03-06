﻿using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.Tilemaps;

public class CameraController : MonoBehaviour
{
    public GameObject toFollow;
    private bool hasRigidBody = false;
    private Entity2D followBody = null;

    private Camera mainCam;
    private PixelPerfectCamera pixelCam;

    [Tooltip("The tilemap that binds the cameras bounds.")]
    public TilemapRenderer tmr;

    private Vector2 screenHalf = Vector2.zero;

    // Start is called before the first frame update
    void Start()
    {
        mainCam = GetComponent<Camera>();
        followBody = toFollow.GetComponent<Entity2D>();
        pixelCam = GetComponent<PixelPerfectCamera>();

        if (followBody != null)
        {
            hasRigidBody = true;
        }

        //Get Screen size
        screenHalf.y = mainCam.orthographicSize * 2.0f;
        screenHalf.x = screenHalf.y * mainCam.aspect;

        screenHalf *= 0.5f;

        screenHalf.x += 24.0f * Physics2DExtra.PIXEL_UNIT;
        screenHalf.y += 24.0f * Physics2DExtra.PIXEL_UNIT;

        transform.position = new Vector3(toFollow.transform.position.x, toFollow.transform.position.y,transform.position.z);
    }

    /// <summary>
    /// Move camera to target, clamps target in tilemap
    /// </summary>
    void FixedUpdate()
    {
        SmoothMoveCameraToTarget();
    }

    private void SmoothMoveCameraToTarget()
    {
        //Get target
        Vector3 target = transform.position;
        if (toFollow != null)
        {
            target = new Vector3(toFollow.transform.position.x,
                toFollow.transform.position.y,
                transform.position.z);
        }

        //Lerp between the cameras pos and the targets
        transform.position = new Vector3(Mathf.Lerp(transform.position.x, target.x, 0.32f),
            Mathf.Lerp(transform.position.y, target.y, 0.16f),
            transform.position.z);

        //Lock position to on tilemap bounds.
        float hWidth = mainCam.orthographicSize;
        float hHeight = mainCam.orthographicSize;

        transform.position = new Vector3(Mathf.Clamp(transform.position.x, tmr.bounds.min.x + screenHalf.x, tmr.bounds.max.x - screenHalf.x),
            Mathf.Clamp(transform.position.y, tmr.bounds.min.y + screenHalf.y, tmr.bounds.max.y - screenHalf.y),
            transform.position.z);
    }
}
