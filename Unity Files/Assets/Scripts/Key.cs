using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    public PlayerController player;
    void OnTriggerEnter2D(Collider2D col)
    {
        player.hasKey = true;
        Destroy(gameObject);
    }
}
