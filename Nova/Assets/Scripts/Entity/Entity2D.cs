using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity2D : MonoBehaviour
{
    const float PIXEL_SIZE = 16.0f;

    [HideInInspector]
    public BoxCollider2D bCollider;

    [HideInInspector]
    public Vector2 Velocity // So it's in pixels, Will definetly not end up confusing i swear
    {
        get {return velocity * PIXEL_SIZE;}
        set {velocity = value / PIXEL_SIZE;}
    }

    private Vector2 velocity;
    private Vector2 subPixelVelocity = new Vector2(0,0);

    [HideInInspector]
    public bool onGround = false;

    [HideInInspector]
    public bool landed = false;

    public LayerMask groundLayer;
    [Tooltip("If the object can move through colliders or not.")]
    public bool doCollisions = true;

    // Start is called before the first frame update
    void Start()
    {
        bCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (doCollisions)
        {
            MoveX(velocity.x);
            MoveY(velocity.y);
        }
        else
        {
            subPixelVelocity.x += velocity.x;
            float move = Mathf.Round(subPixelVelocity.x * PIXEL_SIZE) / PIXEL_SIZE; // In Units 

            if (move != 0)
            {
                subPixelVelocity.x -= move;
                transform.position += new Vector3(move, 0, 0);
            }

            subPixelVelocity.y += velocity.y;
            move = Mathf.Round(subPixelVelocity.y * PIXEL_SIZE) / PIXEL_SIZE; // In Units 

            if (move != 0)
            {
                subPixelVelocity.y -= move;
                transform.position += new Vector3(0, move, 0);
            }
        }

        //Check if on ground
        landed = false;
        if (OnGround())
        {
            if (!onGround)
            {
                landed = true;
            }
            onGround = true;
        }
        else
        {
            onGround = false;
        }

        //Collision();
    }

    private bool OnGround()
    {
        return Physics2DExtra.PlaceMeeting(ref bCollider, new Vector2(0, -1 * Physics2DExtra.PIXEL_UNIT), groundLayer);
    }

    /// <summary>
    /// Returns if a entity is ontop of a collider
    /// </summary>
    /// <param name="collider"></param>
    /// <returns></returns>
    public bool IsRiding(ref Collider2D collider)
    {
        return Physics2DExtra.PlaceMeeting(bCollider, new Vector2(0, -1 * Physics2DExtra.PIXEL_UNIT), groundLayer, collider);
    }

    /// <summary>
    /// Moves the player by amount in the x direction, checks collisions
    /// </summary>
    /// <param name="amount"></param>
    public void MoveX(float amount) // IN UNITS
    {
        subPixelVelocity.x += amount;
        float move = Mathf.Round(subPixelVelocity.x * PIXEL_SIZE) / PIXEL_SIZE; // In Units 

        if(move != 0)
        {
            subPixelVelocity.x -= move;
            float sign = Mathf.Sign(move * PIXEL_SIZE) / PIXEL_SIZE;

            while(move != 0)
            {
                if (!Physics2DExtra.PlaceMeeting(ref bCollider, new Vector2(sign, 0), groundLayer))
                {
                    transform.position += new Vector3(sign, 0, 0);
                    Physics2D.SyncTransforms();
                    move -= sign;
                }
                else
                {
                    velocity.x = 0;
                    subPixelVelocity.x = 0;
                    break;
                }
            }
        }
        Physics2D.SyncTransforms();
    }

    /// <summary>
    /// Same as moveX but in y position
    /// </summary>
    /// <param name="amount"></param>
    public void MoveY(float amount) // IN UNITS, NOT GOING TO CONFUSE ME AT ALL I SWEAR
    {
        subPixelVelocity.y += amount;
        float move = Mathf.Round(subPixelVelocity.y * PIXEL_SIZE) / PIXEL_SIZE; // In Units 

        if (move != 0)
        {
            subPixelVelocity.y -= move;
            float sign = Mathf.Sign(move * PIXEL_SIZE) / PIXEL_SIZE;

            while (move != 0)
            {
                if (!Physics2DExtra.PlaceMeeting(ref bCollider, new Vector2(0, sign), groundLayer))
                {
                    transform.position += new Vector3(0, sign, 0);
                    Physics2D.SyncTransforms();
                    move -= sign;
                }
                else
                {
                    velocity.y = 0;
                    subPixelVelocity.y = 0;
                    break;
                }
            }
        }
        Physics2D.SyncTransforms();
    }
}
