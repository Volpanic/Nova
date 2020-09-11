using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{

    public enum PlayerState
    {
        Gameplay = 0,
        Dead = 1
    };

    const float PIXEL_SCALE = 1.0f / 16.0f;

    public float MoveSpeed = 2.0f;
    public float JumpSpeed = 4.0f;

    public float groundFriction = 0.5f;
    public float airFirction = 0.33f;

    public GameObject burnJumpParticle;
    public GameObject playerDeathParticles;

    public LayerMask groundLayer;
    public GameObject transitionMakerObject;

    public AudioClip jumpSound;
    public AudioClip hurtSound;

    public RectTransform healthBarFront;

    private PlayerState playerState = PlayerState.Gameplay;

    private Entity2D entity;
    private BoxCollider2D bCollider;
    private SpriteRenderer sRenderer;
    private ParticleSystem dustSystem;
    private Animator animator;
    private AudioSource audioSource;

    //Input
    [HideInInspector]
    public bool KeyRight = false;
    [HideInInspector]
    public bool KeyLeft = false;
    [HideInInspector]
    public bool KeyJump = false;
    [HideInInspector]
    public bool KeyJumpHeld = false;
    [HideInInspector]
    public bool KeyJumpRel = false;

    public ControlScheme controls;

    bool initSpawn = true;

    //
    bool isJumping = false;
    bool doFireJump = false;
    float fireJumpAmount = 0.0f;

    private bool recentlyHurt = false;
    private int hurtTimer = 0;
    private float deadTimer = 0;

    private int maxHealth = 3;
    private int currentHealth = 3;
    public  int CurrentHealth { get { return currentHealth; } }

    //Game Feel Things
    private int coyoteTimer = 0;            //Allows a few frames after the leaves the ground to jump
    private int coyoteTimeThreshhold = 6;
    private int jumpBufferTimer = 0;
    private int jumpBufferThreshhold = 6;   //Allows a few frames before landing to que up a jump on land
    private float halfGravBuffer = 0.5f;

    private void Awake()
    {
        if (initSpawn)
        {
            if (GameObject.FindGameObjectsWithTag("Player").Length > 1)
            {
                //Keeps players variables persistant between rooms
                foreach (GameObject oldPlayer in GameObject.FindGameObjectsWithTag("Player"))
                {
                    if (oldPlayer != this.gameObject)
                    {
                        gameObject.transform.localScale = oldPlayer.transform.localScale;
                        gameObject.transform.position = oldPlayer.transform.position;

                        PlayerController oldCont = oldPlayer.GetComponent<PlayerController>();

                        KeyRight = oldCont.KeyRight;
                        KeyLeft = oldCont.KeyLeft;
                        KeyJump = oldCont.KeyJump;
                        KeyJumpHeld = oldCont.KeyJumpHeld;
                        KeyJumpRel = oldCont.KeyJumpRel;

                        currentHealth = oldCont.CurrentHealth;
                        RefeshHealhBar();

                        Destroy(oldPlayer);
                        break;
                    }
                }
            }

            DontDestroyOnLoad(gameObject);
            initSpawn = false;
        }
        else
        {
            return;
        }
    }

    public Entity2D GetEntity()
    {
        return entity;
    }

    // Start is called before the first frame update
    void Start()
    {
        //Get Components
        entity = GetComponent<Entity2D>();
        bCollider = GetComponent<BoxCollider2D>();
        sRenderer = GetComponent<SpriteRenderer>();
        dustSystem = GetComponent<ParticleSystem>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        //Setup Input
        controls = new ControlScheme();
        controls.InGame.Enable();
        controls.InGame.Right.started   += Right_started;
        controls.InGame.Right.canceled  += Right_canceled;
        controls.InGame.Left.started    += Left_started;
        controls.InGame.Left.canceled   += Left_canceled;

        controls.InGame.Jump.started    += Jump_started;
        controls.InGame.Jump.canceled   += Jump_canceled;

        controls.InGame.Respawn.started += Respawn_started;

        //Makes a save at the start if one does not exsist
        if (!File.Exists(Path.Combine(Application.persistentDataPath, SaveGame.fileName)))
        {
            GameSaveData gsd = new GameSaveData(transform.position.x, transform.position.y, this);
            SaveGame.Save(gsd);
        }

        Application.targetFrameRate = 60;
    }

    private void OnDisable()
    {
        if (controls != null)
        {
            controls.InGame.Disable();
        }
    }

    private void OnEnable()
    {
        if (controls != null)
        {
            controls.InGame.Enable();
        }
    }

    private void Respawn_started(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        Respawn();
    }

    public void Jump_canceled(UnityEngine.InputSystem.InputAction.CallbackContext obj) { Jump_canceled(); }
    public void Jump_started(UnityEngine.InputSystem.InputAction.CallbackContext obj) { Jump_started() ; }
    public void Right_started(UnityEngine.InputSystem.InputAction.CallbackContext obj) { Right_started(); }
    public void Right_canceled(UnityEngine.InputSystem.InputAction.CallbackContext obj) { Right_canceled(); }
    public void Left_started(UnityEngine.InputSystem.InputAction.CallbackContext obj) { Left_started(); }
    public void Left_canceled(UnityEngine.InputSystem.InputAction.CallbackContext obj) { Left_canceled(); }

    public void Jump_canceled()
    {
        KeyJumpHeld = false;
        KeyJumpRel = true;
    }

    public void Jump_started()
    {
        KeyJumpHeld = true;
        KeyJump = true;
        KeyJumpRel = false;
    }

    public void Right_started() { KeyRight = true;}
    public void Right_canceled() { KeyRight = false;}
    public void Left_started() { KeyLeft = true;}
    public void Left_canceled() { KeyLeft = false;}

    public void Respawn()
    {
        var load = SaveGame.LoadGame();

        if (load != null)
        {
            GameObject obj = Instantiate(transitionMakerObject,Vector3.zero,Quaternion.identity);
            SceneTransitionMaker maker = obj.GetComponent<SceneTransitionMaker>();
            maker.DoTransitionMovePlayer(load);
        }
        else
        {
            Debug.LogWarning("No Save file to respawn to.");
        }
    }
    
    public void GoToScene(Vector2 targetPos, string sceneName)
    {
        var load = SaveGame.LoadGame();

        if (load != null)
        {
            GameObject obj = Instantiate(transitionMakerObject,Vector3.zero,Quaternion.identity);
            SceneTransitionMaker maker = obj.GetComponent<SceneTransitionMaker>();

            if (maker == null) return;
            maker.DoTransitionMovePlayer(load,sceneName, targetPos);
        }
        else
        {
            Debug.LogWarning("No Save file to respawn to.");
        }
    }

    public void RefillHealth()
    {
        currentHealth = maxHealth;
        RefeshHealhBar();
    }

    public void RefeshHealhBar()
    {
        healthBarFront.localScale = new Vector3((float)currentHealth / (float)maxHealth, healthBarFront.localScale.y, healthBarFront.localScale.z);
    }

    public void Hurt()
    {
        if(!recentlyHurt)
        {
            currentHealth -= 1;
            RefeshHealhBar();
            currentHealth = Mathf.Clamp(currentHealth,0,maxHealth);

            if(currentHealth <= 0)
            {
                Instantiate(playerDeathParticles, transform);
                playerState = PlayerState.Dead;
                animator.Play("ani_player_dead");
            }

            audioSource.PlayOneShot(hurtSound);
            recentlyHurt = true;
            hurtTimer = 90;
        }
    }

    void FixedUpdate()
    {
        switch(playerState)
        {
            case PlayerState.Gameplay:
            {
                GameplayState();
            break;
            }
            
            case PlayerState.Dead:
            {
                DeadState();
            break;
            }
        }
    }

    // Update is called once per frame
    void GameplayState()
    {
        float fricc = groundFriction;
        if (!entity.onGround) fricc = airFirction;

        float grav = 0.2f;

        //Lock it to this frame
        if(doFireJump)
        {
            entity.Velocity = new Vector2(entity.Velocity.x, fireJumpAmount);
            coyoteTimer = 0;
            jumpBufferTimer = 0;
            isJumping = false;
            doFireJump = false;

            if (gameObject.transform.childCount > 0)
            {
                Destroy(gameObject.transform.GetChild(0).gameObject);
            }

            GameObject obj = GameObject.Instantiate(burnJumpParticle, transform);
        }

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

        //Landed Dust
        if(entity.landed)
        {
            int amount = Random.Range(1,3);

            for (int i = 0; i < amount; i++)
            {
                //Set positions and velocity.
                ParticleSystem.EmitParams ep = new ParticleSystem.EmitParams();
                ep.position = new Vector3(transform.position.x + (Random.Range(-4, 4) * Physics2DExtra.PIXEL_UNIT),
                    Physics2DExtra.Bottom(bCollider), transform.position.z);

                //Set velcoyty to be fairly random
                ep.velocity = new Vector3(Random.Range(-1,1), Random.Range(0.5f, 1f), ep.velocity.z);

                //Particle emit
                dustSystem.Emit(ep, 1);
            } 
        }

        if(entity.onGround)
        {
            //Set this so it can count down when not on ground
            coyoteTimer = coyoteTimeThreshhold;
            isJumping = false;

            if (KeyJump || jumpBufferTimer > 0)
            {
                Jump();
            }

            //create dust if moving
            if(entity.Velocity.x != 0)
            {
                if (Random.Range(0, 20) >= 19)
                {
                    //Set positions and velocity.
                    ParticleSystem.EmitParams ep = new ParticleSystem.EmitParams();
                    ep.position = new Vector3(transform.position.x + (Random.Range(-4,4) * Physics2DExtra.PIXEL_UNIT),
                        Physics2DExtra.Bottom(bCollider),transform.position.z);

                    //Set velcoyty to be fairly random
                    ep.velocity = (entity.Velocity * -Random.Range(0.5f,1));
                    ep.velocity = new Vector3(ep.velocity.x, Random.Range(0.5f, 1f), ep.velocity.z);

                    //Particle emit
                    dustSystem.Emit(ep, 1);
                }
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
            if(KeyJumpRel && isJumping)
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

        if(recentlyHurt)
        {
            hurtTimer -= 1;

            if(hurtTimer % 5 == 0)
            {
                sRenderer.enabled = true;
            }
            else
            {
                sRenderer.enabled = false;
            }

            if (hurtTimer <= 0)
            {
                sRenderer.enabled = true;
                recentlyHurt = false;
            }
        }

        Animation();

        //These should only be active one frame
        KeyJump = false;
        KeyJumpRel = false;
    }

    void DeadState()
    {
        entity.Velocity = new Vector2(entity.Velocity.x, entity.Velocity.y - 0.2f);
        entity.Velocity = new Vector2(Numbers.Approach(entity.Velocity.x, 0.0f, groundFriction), entity.Velocity.y);

        deadTimer += Time.fixedDeltaTime;

        if(deadTimer >= 2.0f)
        {
            deadTimer = -10000.0f;
            Respawn();
        }
    }

    public void Jump()
    {
        entity.Velocity = new Vector2(entity.Velocity.x, JumpSpeed);
        coyoteTimer = 0;
        jumpBufferTimer = 0;
        isJumping = true;
       // audioSource.PlayOneShot(jumpSound);

        int amount = Random.Range(1, 3);

        for (int i = 0; i < amount; i++)
        {
            //Set positions and velocity.
            ParticleSystem.EmitParams ep = new ParticleSystem.EmitParams();
            ep.position = new Vector3(transform.position.x + (Random.Range(-4, 4) * Physics2DExtra.PIXEL_UNIT),
                Physics2DExtra.Bottom(bCollider), transform.position.z);

            //Set velcoyty to be fairly random
            ep.velocity = (entity.Velocity * -Random.Range(0.25f, 0.5f));

            //Particle emit
            dustSystem.Emit(ep, 1);
        }
    }

    public void BurnJump(float jumpForce)
    {
        doFireJump = true;
        fireJumpAmount = jumpForce;
    }

    public void Animation()
    {
        if (entity.Velocity.x != 0)
        {
            sRenderer.flipX = (Mathf.Sign(entity.Velocity.x) == -1) ? true : false;
        }

        if (entity.onGround)
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
