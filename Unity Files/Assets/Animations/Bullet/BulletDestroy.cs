﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDestroy : MonoBehaviour
{
    public void DestroyBullet()
    {
        Destroy(gameObject.transform.parent.gameObject);
    }
}
