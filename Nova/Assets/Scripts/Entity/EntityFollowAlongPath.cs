using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EntityFollowAlongPath : MonoBehaviour
{

    public enum PathType
    {
        Closed,
        Open,
        Reverse
    };

    Entity2D entitiy = null;
    MoveingSolid movingSolid = null;

    [Tooltip("If the entity should rotate there Z to face where they are moving")]
    public bool rotateWith = false;
    [Tooltip("What to do at the end of a path.")]
    public PathType pathType = PathType.Closed;

    [Tooltip("Position of each node path")]
    public List<Vector2> pointPositions;

    int listIndex = 0;
    int listDir = 1;

    [Tooltip("How fast to move along path")]
    public float speed = 1;

    

    // Start is called before the first frame update
    void Start()
    {
        entitiy = GetComponent<Entity2D>();

        if (entitiy == null)
        {
            movingSolid = GetComponent<MoveingSolid>();

            if (movingSolid == null)
            {
                Debug.LogWarning("Object : " + gameObject.name + " needs and Entity2D or MoveingSolid to use EntityFollowAlongPath.");
            }
        }

        speed = speed * Physics2DExtra.PIXEL_UNIT;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(listIndex == -4)
        {
            return;
        }

        float dist = Vector2.Distance(transform.position, pointPositions[listIndex]);
        
        if(dist == 0)
        {
            listIndex += listDir;

            if (listIndex >= pointPositions.Count || listIndex < 0)
            {
                //React diffrec to diffrent path types.
                switch (pathType)
                {
                    case PathType.Closed:
                        {
                            listIndex = 0;
                            break;
                        }

                    case PathType.Reverse:
                        {
                            listDir = -listDir;
                            listIndex += listDir; // set back to last node
                            listIndex += listDir; // set target node before last node
                            break;
                        }

                    case PathType.Open:
                        {
                            listIndex = -4;
                            break;
                        }
                }
            }
        }

        //if (dist <= speed)
        //{
        //    Vector2 movement = (pointPositions[listIndex] - (Vector2)transform.position);

        //    entitiy.Velocity = Vector2.MoveTowards((Vector2)transform.position, pointPositions[listIndex],speed) - (Vector2)transform.position;
        //}
        //else
        //{
        //    Vector2 movement = (pointPositions[listIndex] - (Vector2)transform.position).normalized * speed;
        //    entitiy.Velocity = movement;
        //}

        if (entitiy != null)
        {
            entitiy.Velocity = (Vector2.MoveTowards((Vector2)transform.position, pointPositions[listIndex], speed) - (Vector2)transform.position) * Physics2DExtra.PIXEL_SIZE;
        }
        else
        {
            movingSolid.Velocity = (Vector2.MoveTowards((Vector2)transform.position, pointPositions[listIndex], speed) - (Vector2)transform.position) * Physics2DExtra.PIXEL_SIZE;
        }

        if (rotateWith)
        {
            transform.eulerAngles = Vector3.forward * (Mathf.Atan2(entitiy.Velocity.y,entitiy.Velocity.x) * Mathf.Rad2Deg);
        }

    }
}
