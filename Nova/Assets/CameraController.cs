using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject toFollow;
    private bool hasRigidBody = false;
    private Rigidbody2D followBody;

    // Start is called before the first frame update
    void Start()
    {
        followBody = gameObject.GetComponent<Rigidbody2D>();
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
            target = new Vector3(toFollow.transform.position.x + (followBody.velocity.x * 4.0f),
                toFollow.transform.position.y + (followBody.velocity.y * 4.0f),
                transform.position.z);
        }
        else
        {
            target = new Vector3(toFollow.transform.position.x,
                toFollow.transform.position.y,
                transform.position.z);
        }

        transform.position = Vector3.Lerp(transform.position,target,0.32f);
    }
}
