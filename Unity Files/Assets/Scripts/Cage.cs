using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cage : MonoBehaviour
{

    public Pushable one;
    public Pushable two;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!one.isPushable && !two.isPushable)
        {
            Destroy(gameObject);
        }
    }
}
