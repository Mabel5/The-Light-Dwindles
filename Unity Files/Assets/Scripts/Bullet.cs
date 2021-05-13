using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Enemy")
        {
            collision.transform.GetComponent<Enemy>().health -= 1;
        }

        gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        gameObject.transform.GetChild(0).GetComponent<Animator>().Play("bullet");
    }

    public void FinishExplodeAnimation()
    {
        Destroy(gameObject.transform.parent);
    }
}
