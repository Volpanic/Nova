﻿using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HurtPlayerOnTouch : MonoBehaviour
{
    [Tooltip("An event to play after the player is hit")]
    public UnityEvent hurtPlayerEvent = new UnityEvent();

    [Tooltip("What collider should interact with the player")]
    public Collider2D hurtCollider;

    public LayerMask entityLayer;
    private Collider2D playerCollider;

    private void Start()
    {
        playerCollider = GameObject.FindGameObjectWithTag("Player").GetComponent<BoxCollider2D>(); 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerController>().Hurt();
            hurtPlayerEvent.Invoke();
        }
    }
}
