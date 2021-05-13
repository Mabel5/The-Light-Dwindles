using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public Transform player;
    public float lerpSpeed;

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = gameObject.transform.position;
        pos.x = player.position.x;
        pos.y = player.position.y;

        gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, pos, lerpSpeed);
    }
}
