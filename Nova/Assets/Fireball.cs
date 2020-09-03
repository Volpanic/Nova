using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Fireball : MonoBehaviour
{
    Entity2D entity;

    public Vector2 dirVec = Vector2.right;
    public float Speed = 1;


    // Start is called before the first frame update
    void Start()
    {
        entity = GetComponent<Entity2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //entity.Velocity = dirVec * Speed;

    }

    public void OnHurtPlayer()
    {
        Destroy(this.gameObject);
    }
}
