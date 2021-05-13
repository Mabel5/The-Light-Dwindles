using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinController : MonoBehaviour
{
    public bool isTriggerable = true;
    public void OnFinishPickupAnimation()
    {
        Destroy(transform.parent.gameObject);
    }
    
    public void PlayExplosion()
    {
        gameObject.GetComponent<Animator>().Play("explode");
    }
}
