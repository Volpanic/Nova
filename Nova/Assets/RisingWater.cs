using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RisingWater : MonoBehaviour
{
    private PlayerController player = null;
    float risingSpeed = 0.02f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position += new Vector3(0, risingSpeed, 0);

        if (player != null)
        {
            if (player.gameObject != null)
            {
                if (player.GetEntity().bCollider.bounds.max.y < transform.position.y)
                {
                    player.Hurt();
                }
            }
        }
        else
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        }
        
    }
}
