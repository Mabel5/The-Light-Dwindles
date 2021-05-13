using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Light2D = UnityEngine.Experimental.Rendering.Universal.Light2D;

public class Pushable : MonoBehaviour
{
    public bool isPushable = true;

    void Update()
    {
        if (gameObject.transform.position.y >= 4 && isPushable)
        {
            gameObject.transform.GetChild(0).GetComponent<Light2D>().intensity += 3f;
            isPushable = false; 
        }
    }
}
