using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Fireball : MonoBehaviour
{
    Entity2D entity;
    PlayerController playerEntity = null;

    public Vector2 dirVec = Vector2.right;
    public float Speed = 1;
    public ParticleSystem pSystem = null;




    // Start is called before the first frame update
    void Start()
    {
        entity = GetComponent<Entity2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //entity.Velocity = dirVec * Speed;
        if(pSystem != null)
        {
            //Emit fire particle
            if (Random.Range(0, 20) >= 10)
            {
                //Set positions and velocity.
                ParticleSystem.EmitParams ep = new ParticleSystem.EmitParams();
                ep.position = transform.position;
                ep.velocity = (entity.Velocity * -1);
                ep.velocity = new Vector3(ep.velocity.x,Random.Range(-(Mathf.PI/8.0f), (Mathf.PI / 8.0f)),ep.velocity.z);

                //Particle emit
                pSystem.Emit(ep, Mathf.FloorToInt(entity.Velocity.magnitude));
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (playerEntity == null)
            {
                playerEntity = collision.gameObject.GetComponent<PlayerController>();
            }

            playerEntity.BurnJump(7);

            if (pSystem != null)
            {
                //Emit fire particle
                for (int i = 0; i < Random.Range(6, 10); i++)
                {
                    //Set positions and velocity.
                    ParticleSystem.EmitParams ep = new ParticleSystem.EmitParams();
                    ep.position = transform.position;
                    ep.velocity = new Vector2(Random.Range(-1, 1), Random.Range(-1, 1));

                    //Particle emit
                    pSystem.Emit(ep, 1);
                }
            }
        }
    }

    public void OnHurtPlayer()
    {
        Destroy(this.gameObject);
    }
}
