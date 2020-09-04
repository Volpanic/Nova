﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePoint : MonoBehaviour
{

    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //collision.gameObject.GetComponent<PlayerController>().
            StartFire();
        }
    }

    public void StartFire()
    {
        animator.Play("SaveTorchFlicker");

        //Only have one fire active at once
        SavePoint[] points = FindObjectsOfType<SavePoint>();

        foreach(SavePoint point in points)
        {
            if(point != this)
            {
                point.StopFire();
            }
        }

    }

    public void StopFire()
    {
        animator.Play("ani_save_torch_idle");
    }
}
