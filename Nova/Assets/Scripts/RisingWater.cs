using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RisingWater : MonoBehaviour
{
    private PlayerController player = null;
    public GameObject waterStretchObject;
    public float initYPos = 0;
    float risingSpeed = 0.02f;

    // Start is called before the first frame update
    void Start()
    {
        initYPos = transform.position.y;
        waterStretchObject.transform.position = transform.position;
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

        waterStretchObject.transform.localScale = new Vector3(waterStretchObject.transform.localScale.x,
            (((transform.position.y + risingSpeed) - (Physics2DExtra.PIXEL_UNIT * (Physics2DExtra.PIXEL_SIZE/2))) - initYPos) * 16,
            waterStretchObject.transform.localScale.z);

        risingSpeed += 0.000002f;
    }
}
