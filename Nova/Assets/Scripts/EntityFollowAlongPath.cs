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

    Entity2D entitiy;

    public bool rotateWith = false;
    public PathType pathType = PathType.Closed;

    
    public List<Vector2> pointPositions;

    int listIndex = 0;
    int listDir = 1;
    public float speed = 1;

    

    // Start is called before the first frame update
    void Start()
    {
        entitiy = GetComponent<Entity2D>();

        if (entitiy == null)
        {
            Debug.LogWarning("Object : " + gameObject.name + " needs and Entity2D to use EntityFollowAlongPath.");
        }
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

        if (dist <= speed)
        {
            Vector2 movement = (pointPositions[listIndex] - (Vector2)transform.position);
            entitiy.Velocity = movement;
            Debug.Log("Dist  " + dist.ToString() + " Mag  : " + movement.magnitude.ToString());

        }
        else
        {
            Vector2 movement = (pointPositions[listIndex] - (Vector2)transform.position).normalized * speed;
            entitiy.Velocity = movement;
        }

        if(rotateWith)
        {
            transform.eulerAngles = Vector3.forward * (Mathf.Atan2(entitiy.Velocity.y,entitiy.Velocity.x) * Mathf.Rad2Deg);
        }

    }
}
