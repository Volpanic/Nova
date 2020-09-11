using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveingSolid : MonoBehaviour
{
    [HideInInspector]
    public Vector2 Velocity // So it's in pixels, Will definetly not end up confusing i swear
    {
        get { return velocity * Physics2DExtra.PIXEL_SIZE; }
        set { velocity = value / Physics2DExtra.PIXEL_SIZE; }
    }

    private Vector2 velocity;
    private Vector2 subPixelVelocity = new Vector2(0, 0);
    private LayerMask originalLayerMask; 

    public LayerMask entitiyLayer;
    public Collider2D gCollider; // Public so it can work with more than box

    // Start is called before the first frame update
    void Start()
    {
        originalLayerMask = gameObject.layer;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move(velocity.x,velocity.y);
    }

    /// <summary>
    /// Shows the cornors of the platform
    /// </summary>
    public void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos2D.DrawCircle(new Vector2(Physics2DExtra.Left(gCollider), Physics2DExtra.Top(gCollider)), 0.1f);

        Gizmos.color = Color.red;
        Gizmos2D.DrawCircle(new Vector2(Physics2DExtra.Right(gCollider), Physics2DExtra.Top(gCollider)), 0.1f);

        Gizmos.color = Color.green;
        Gizmos2D.DrawCircle(new Vector2(Physics2DExtra.Left(gCollider), Physics2DExtra.Bottom(gCollider)), 0.1f);

        Gizmos.color = Color.yellow;
        Gizmos2D.DrawCircle(new Vector2(Physics2DExtra.Right(gCollider), Physics2DExtra.Bottom(gCollider)), 0.1f);
    }

    /// <summary>
    /// Moves the platform, pushes entities
    /// </summary>
    /// <param name="xAmount"></param>
    /// <param name="yAmount"></param>
    private void Move(float xAmount, float yAmount)
    {
        subPixelVelocity.x += xAmount;
        subPixelVelocity.y += yAmount;

        float moveX = Mathf.Round(subPixelVelocity.x * Physics2DExtra.PIXEL_SIZE) / Physics2DExtra.PIXEL_SIZE; // In Units 
        float moveY = Mathf.Round(subPixelVelocity.y * Physics2DExtra.PIXEL_SIZE) / Physics2DExtra.PIXEL_SIZE; // In Units 

        if(moveX != 0 || moveY != 0)
        {
            List<Entity2D> riding = GetAllRidingEntities();
            Entity2D[] AllEntities = FindObjectsOfType<Entity2D>();

            //May need to change
            gCollider.isTrigger = true;

            //X
            if (moveX != 0)
            {
                //Moveplatform
                subPixelVelocity.x -= moveX; 
                transform.position += new Vector3(moveX, 0, 0);
                Physics2D.SyncTransforms();

                if (moveX > 0)
                {
                    foreach(Entity2D ent in AllEntities)
                    {
                        if (Physics2DExtra.PlaceMeeting(gCollider, Vector2.zero, entitiyLayer, ent.bCollider,true))
                        {
                            ent.MoveX(Physics2DExtra.Right(gCollider) - Physics2DExtra.Left(ent.bCollider));
                        }
                        else if(riding.Contains(ent))
                        {
                            ent.MoveX(moveX);
                        }
                    }
                }
                else if(moveX < 0)
                {
                    foreach(Entity2D ent in AllEntities)
                    {
                        if (Physics2DExtra.PlaceMeeting(gCollider, Vector2.zero, entitiyLayer, ent.bCollider,true))
                        {
                            ent.MoveX(Physics2DExtra.Left(gCollider)  - Physics2DExtra.Right(ent.bCollider));
                        }
                        else if(riding.Contains(ent))
                        {
                            ent.MoveX(moveX);
                        }
                    }
                }
            }

            //Y
            if (moveY != 0)
            {
                //Moveplatform
                subPixelVelocity.y -= moveY;
                transform.position += new Vector3(0, moveY, 0);
                Physics2D.SyncTransforms();

                if (moveY > 0)
                {
                    foreach (Entity2D ent in AllEntities)
                    {
                        if (Physics2DExtra.PlaceMeeting(gCollider, Vector2.zero, entitiyLayer, ent.bCollider,true))
                        {
                            ent.MoveY(Physics2DExtra.Top(gCollider) - Physics2DExtra.Bottom(ent.bCollider));
                        }
                        else if (riding.Contains(ent))
                        {
                            ent.MoveY(moveY);
                        }
                    }
                }
                else
                {
                    foreach (Entity2D ent in AllEntities)
                    {
                        if (Physics2DExtra.PlaceMeeting(gCollider, Vector2.zero, entitiyLayer, ent.bCollider,true))
                        {
                            ent.MoveY(Physics2DExtra.Bottom(gCollider) - Physics2DExtra.Top(ent.bCollider));
                        }
                        else if (riding.Contains(ent))
                        {
                            ent.MoveY(moveY);
                        }
                    }
                }
            }
                //May need to change
                gCollider.isTrigger = false;
        }
    }

    /// <summary>
    /// Return all entities on top of the platform.
    /// </summary>
    /// <returns></returns>
    private List<Entity2D> GetAllRidingEntities()
    {
        Entity2D[] a = FindObjectsOfType<Entity2D>();
        List<Entity2D> returnList = new List<Entity2D>();

        foreach(Entity2D ent in a)
        {
            if(ent.IsRiding(ref gCollider))
            {
                
                returnList.Add(ent);
            }
        }

        return returnList;
    }
}
