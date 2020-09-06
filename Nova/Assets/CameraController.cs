using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CameraController : MonoBehaviour
{
    public GameObject toFollow;
    private bool hasRigidBody = false;
    private Entity2D followBody;

    public TilemapRenderer tmr;

    // Start is called before the first frame update
    void Start()
    {
        followBody = gameObject.GetComponent<Entity2D>();
        if(followBody != null)
        {
            hasRigidBody = true;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 target = transform.position;
        
        if (hasRigidBody)
        {
            target = new Vector3(toFollow.transform.position.x + (followBody.Velocity.x * 5.0f),
                toFollow.transform.position.y + (followBody.Velocity.y * 5.0f),
                transform.position.z);
        }
        else
        {
            target = new Vector3(toFollow.transform.position.x,
                toFollow.transform.position.y,
                transform.position.z);
        }

        transform.position = Vector3.Lerp(transform.position,target,0.32f);


        transform.position = new Vector3(Mathf.Clamp(transform.position.x,tmr.bounds.min.x,tmr.bounds.max.x),
            Mathf.Clamp(transform.position.y, tmr.bounds.min.y, tmr.bounds.max.y),
            transform.position.z);
    }
}
