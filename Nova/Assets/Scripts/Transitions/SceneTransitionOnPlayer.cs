using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTransitionOnPlayer : MonoBehaviour
{
    private BoxCollider2D bCollider;
    private bool activated = false;

    public Vector2 targetPos;
    public string targetScene;

    // Start is called before the first frame update
    void Start()
    {
        bCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(!activated && collision.gameObject.tag == "Player")
        {
            activated = true;
            collision.gameObject.GetComponent<PlayerController>().GoToScene(targetPos,targetScene);
        }
    }
}
