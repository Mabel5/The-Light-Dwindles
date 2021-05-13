using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal.Internal;

public class Enemy : MonoBehaviour
{

    public int health;
    public float moveSpeed;
    public float damage;
    public Transform player;
    public float minDist;
    public float maxDist;
    public int attackCooldownValue = 50;
    public int attackCooldown;

    // Start is called before the first frame update
    void Start()
    {
       attackCooldown = 0;
    }

    private bool isDead = false;
    void Update()
    {
        if (health <= 0)
        {
            isDead = true;
            gameObject.GetComponent<Collider2D>().enabled = false;
            gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
            gameObject.GetComponent<Animator>().SetBool("isDead", true);
        }
    }

    private void FixedUpdate()
    {
        if (!isDead)
        {
            if (player.position.x > transform.position.x)
            {
                transform.eulerAngles = new Vector2(0, 0);
            }
            else
            {
                transform.eulerAngles = new Vector2(0, 180);
            }

            float dist = Vector2.Distance(transform.position, player.position);
            if (dist >= minDist && dist <= maxDist)
            {
                gameObject.transform.GetComponent<Animator>().SetBool("isWalking", true);
                transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
                transform.position = new Vector3(transform.position.x, transform.position.y, -0.3f);

                if (dist > minDist)
                {
                    gameObject.transform.GetComponent<Animator>().SetBool("isAttacking", false);
                }
            }
            else
            {
                gameObject.transform.GetComponent<Animator>().SetBool("isWalking", false);

                if (dist <= minDist)
                {
                    gameObject.transform.GetComponent<Animator>().SetBool("isAttacking", true);

                    if (attackCooldown <= 0)
                    {
                        player.GetComponent<PlayerController>().decreaseLife(damage);
                        attackCooldown = attackCooldownValue;
                    }
                }
                else
                {
                    gameObject.transform.GetComponent<Animator>().SetBool("isAttacking", false);
                }
            }

            if (attackCooldown > 0)
            {
                attackCooldown--;
            }
        }
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}
