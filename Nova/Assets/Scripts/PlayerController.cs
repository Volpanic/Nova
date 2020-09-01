﻿using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    const float PIXEL_SCALE = 1.0f / 16.0f;

    public float MoveSpeed = 2.0f;
    public float JumpSpeed = 4.0f;

    public float groundFriction = 0.5f;
    public float airFirction = 0.33f;

    public LayerMask groundLayer;

    private Entity2D entity;
    private BoxCollider2D bCollider;
    private SpriteRenderer sRenderer;
    private Animator animator;

    //Input
    bool KeyRight = false;
    bool KeyLeft = false;
    bool KeyJump = false;
    bool KeyJumpHeld = false;
    bool KeyJumpRel = false;

    //
    bool isJumping = false;

    //Game Feel Things
    private int coyoteTimer = 0;            //Allows a few frames after the leaves the ground to jump
    private int coyoteTimeThreshhold = 6;
    private int jumpBufferTimer = 0;
    private int jumpBufferThreshhold = 6;   //Allows a few frames before landing to que up a jump on land
    private float halfGravBuffer = 0.5f;

    private bool OnGround()
    {
        return Physics2DExtra.PlaceMeeting(ref bCollider, new Vector2(0, -1 * PIXEL_SCALE), groundLayer);
    }

    // Start is called before the first frame update
    void Start()
    {
        entity = GetComponent<Entity2D>();
        bCollider = GetComponent<BoxCollider2D>();
        sRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    public void OnDrawGizmos()
    {

    }

    private void Update()
    {
        KeyRight = (Input.GetAxisRaw("Horizontal") > 0.25f);
        KeyLeft = (Input.GetAxisRaw("Horizontal") < -0.25f);
        if(!KeyJump) KeyJump = (Input.GetButtonDown("Jump"));
        if(!KeyJumpHeld) KeyJumpHeld = (Input.GetButton("Jump"));
        if(!KeyJumpRel) KeyJumpRel = (Input.GetButtonUp("Jump"));
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        bool onGround = OnGround();
        float fricc = groundFriction;
        if (!onGround) fricc = airFirction;

        float grav = 0.2f;

        //Mvoement
        if (KeyRight)
        {
            entity.Velocity = new Vector2(Numbers.Approach(entity.Velocity.x,MoveSpeed, fricc), entity.Velocity.y);
        }
        else if(KeyLeft)
        {
            entity.Velocity = new Vector2(Numbers.Approach(entity.Velocity.x, -MoveSpeed, fricc), entity.Velocity.y);
        }
        else
        {
            entity.Velocity = new Vector2(Numbers.Approach(entity.Velocity.x, 0.0f, fricc), entity.Velocity.y);
        }

        if(onGround)
        {
            //Set this so it can count down when not on ground
            coyoteTimer = coyoteTimeThreshhold;
            isJumping = false;

            if (KeyJump || jumpBufferTimer > 0)
            {
                Jump();
            }

        }
        else // Not on ground
        {
            if (KeyJump)
            {
                if (coyoteTimer > 0)
                {
                    Jump();
                }
                else // Set jump buffer
                {
                    jumpBufferTimer = jumpBufferThreshhold;
                }
            }

            if(KeyJumpHeld && Mathf.Abs(entity.Velocity.y) <= halfGravBuffer)
            {
                grav *= 0.5f;
            }

            //Variable jump height
            if(isJumping && KeyJumpRel)
            {
                if(entity.Velocity.y > 0)
                {
                    if(entity.Velocity.y >= JumpSpeed/4.0f)
                    {
                        entity.Velocity = new Vector2(entity.Velocity.x,JumpSpeed / 4.0f);
                    }
                }
                isJumping = false;
            }

            coyoteTimer = Numbers.Approach(coyoteTimer,0,1);
        }
        jumpBufferTimer = Numbers.Approach(jumpBufferTimer, 0, 1);

        //Gravity
        if (entity.Velocity.y > -10)
        {
            entity.Velocity = new Vector2(entity.Velocity.x, entity.Velocity.y - grav);
        }

        //Debug.Log(entity.Velocity.ToString() + " : " + overFlowVelocity.ToString()); ;

        Animation();
        KeyJump = false;
        KeyJumpHeld = false;
        KeyJumpRel = false;
    }

    public void Jump()
    {
        entity.Velocity = new Vector2(entity.Velocity.x, JumpSpeed);
        coyoteTimer = 0;
        jumpBufferTimer = 0;
        isJumping = true;
    }

    public void Animation()
    {
        if (entity.Velocity.x != 0)
        {
            sRenderer.flipX = (Mathf.Sign(entity.Velocity.x) == -1) ? true : false;
        }

        if (OnGround())
        {
            if(entity.Velocity.x != 0)
            {
                animator.Play("ani_player_walk");
            }
            else
            {
                animator.Play("ani_player_idle");
            }
        }
        else
        {
            if(entity.Velocity.y > 0) // Jumping
            {
                animator.Play("ani_player_jump");
            }
            else
            {
                animator.Play("ani_player_fall");
            }
        }
    }
}
