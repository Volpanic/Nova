using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityRotateAround : MonoBehaviour
{
    Entity2D entitiy;

    public float Radius = 1;
    public float Speed = 1;
    public int Percision = 8;
    public bool rotateWith = false;

    private List<Vector2> pointPositions = new List<Vector2>();
    private Vector3 initPos;
    private int listIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        entitiy = GetComponent<Entity2D>();

        if (entitiy == null)
        {
            Debug.LogWarning("Object : " + gameObject.name + " needs and Entity2D to use EntityFollowAlongPath.");
        }


        initPos = transform.position;

        float inc = (Mathf.PI * 2.0f) / Percision;
        float cur = 0;

        for (int i = 0; i <= Percision; i++)
        {
            cur += inc;
            Vector2 newPos = new Vector2(Mathf.Cos(cur), Mathf.Sin(cur)) * Radius;
            pointPositions.Add((Vector2)initPos + newPos);
        }

        transform.position = pointPositions[0];
        Speed = Speed * Physics2DExtra.PIXEL_UNIT;
    }

    private void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            Gizmos2D.DrawCircle(initPos, Radius, Percision);
        }
        else
        {
            Gizmos2D.DrawCircle(transform.position, Radius, Percision);
        }
    }

    void FixedUpdate()
    {
        float dist = Vector2.Distance(transform.position, pointPositions[listIndex]);

        if (dist <= Physics2DExtra.PIXEL_UNIT)
        {
            listIndex++;

            if (listIndex >= pointPositions.Count || listIndex < 0)
            {
                //React diffrec to diffrent path types.
                listIndex = 0;
            }
        }


        entitiy.Velocity = (Vector2.MoveTowards((Vector2)transform.position, pointPositions[listIndex], Speed) - (Vector2)transform.position) * Physics2DExtra.PIXEL_SIZE;

        if (rotateWith)
        {
            transform.eulerAngles = Vector3.forward * (Mathf.Atan2(entitiy.Velocity.y, entitiy.Velocity.x) * Mathf.Rad2Deg);
        }
    }
}

