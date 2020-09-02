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

    public LayerMask groundLayer;
    public bool doCollisions = true;

    // Start is called before the first frame update
    void Start()
    {
        bCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        MoveX(velocity.x);
        MoveY(velocity.y);
        //Collision();
    }

    public bool IsRiding(ref Collider2D collider)
    {
        return Physics2DExtra.PlaceMeeting(bCollider, new Vector2(0, -1 * Physics2DExtra.PIXEL_UNIT), groundLayer, collider);
    }


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

    public void Collision()
    {
        if(!doCollisions)
        {
            transform.position += (Vector3)velocity;
            return;
        }

        //X Collision
        if(velocity.x != 0 && Physics2DExtra.PlaceMeeting(ref bCollider,new Vector2(velocity.x,0),groundLayer))
        {
            //Multiplication because sign only works with ints(Unity units aren't pixels)
            float sign = Mathf.Sign(velocity.x * PIXEL_SIZE) / PIXEL_SIZE;
            while (sign != 0 && !Physics2DExtra.PlaceMeeting(ref bCollider, new Vector2(sign, 0), groundLayer))
            {
                transform.position += new Vector3(sign, 0, 0);

                sign = Mathf.Sign(velocity.x * PIXEL_SIZE) / PIXEL_SIZE;
            }
            velocity.x = 0;  
        }
        transform.position += new Vector3(velocity.x,0,0);
        
        //Y Collision
        if(velocity.y != 0 && Physics2DExtra.PlaceMeeting(ref bCollider,new Vector2(0, velocity.y),groundLayer))
        {
            //Multiplication because sign only works with ints(Unity units aren't pixels)
            float sign = Mathf.Sign(velocity.y * PIXEL_SIZE) / PIXEL_SIZE;
            while (sign != 0 && !Physics2DExtra.PlaceMeeting(ref bCollider, new Vector2(0, sign), groundLayer))
            {
                transform.position += new Vector3(0, sign, 0);

                sign = Mathf.Sign(velocity.y * PIXEL_SIZE) / PIXEL_SIZE;
            }
            velocity.y = 0;
            
        }
        transform.position += new Vector3(0, velocity.y, 0);
    }
}
