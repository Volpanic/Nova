using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    public float MaxTime = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MaxTime -= Time.deltaTime;

        if(MaxTime <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
