using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterDropletSpawner : MonoBehaviour
{
    public float delayInSeconds = 1;
    public GameObject waterDroplet = null;

    private Animator anim = null;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        anim.speed = delayInSeconds;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnDroplet()
    {
        Instantiate(waterDroplet,transform);
    }
}
