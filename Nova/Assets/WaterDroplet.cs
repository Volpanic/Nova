using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterDroplet : MonoBehaviour
{
    public LayerMask groundLayer;
    public GameObject splashParticle = null;

    private Entity2D entity;
    private BoxCollider2D bCollider;

    private float gravity = -0.2f;
    private float terminalGravity = -10.0f;

    private float graceTimer = 0.25f; // Because droplets spawn in a wall give it a few seconds that it won't be destroyed;
    private float maxLifeTimer = 10f; // Incase theres no ground, so the droplet doesn't just fall forever

    // Start is called before the first frame update
    void Start()
    {
        entity = GetComponent<Entity2D>();
        bCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        graceTimer = Numbers.Approach(graceTimer,0,Time.deltaTime);
        maxLifeTimer = Numbers.Approach(maxLifeTimer, 0,Time.deltaTime);

        if(maxLifeTimer <= 0)
        {
            DestorySelf();
        }
    }

    private void FixedUpdate()
    {
        if(entity.Velocity.y > terminalGravity)
        {
            entity.Velocity = new Vector2(entity.Velocity.x,entity.Velocity.y + gravity);
        }

        if (graceTimer <= 0 && Physics2D.IsTouchingLayers(bCollider,groundLayer))
        {
            DestorySelf();
        }
    }

    public void DestorySelf()
    {
        Destroy(gameObject);
        Instantiate(splashParticle, transform.position, Quaternion.identity);
    }
}
