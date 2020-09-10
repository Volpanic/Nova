using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HurtPlayerOnTouch : MonoBehaviour
{
    public UnityEvent hurtPlayerEvent = new UnityEvent();
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
