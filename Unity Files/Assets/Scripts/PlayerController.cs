using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;
using Light2D = UnityEngine.Experimental.Rendering.Universal.Light2D;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class PlayerController : MonoBehaviour
{

    public int moveSpeed;
    public float lerpSpeed;
    public float bulletSpeed;
    public GameObject bulletPrefab;
    public bool hasKey = false;
    public float walkThreshold;
    public Animator anim;
    public Light2D playerLight;
    public Image healthBar;
    public Image healthBar2;
    public float lifeIncrease;
    public Text deathText;
    public Text lightText1;
    public Text lightText2;
    public Text keyText;
    public AudioClip die;
    public AudioClip pickup;
    public AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        lightText1.GetComponent<Animation>().Play("fadeIn");
        lightText1.GetComponent<Animation>().Play("fadeIn");
        healthBar.GetComponent<Animation>().Play("healthFadeIn");
        healthBar2.GetComponent<Animation>().Play("healthFadeIn");
        playerLight.GetComponent<Animation>().Play("lightStart");

        audioSource = GetComponent<AudioSource>();

    }

    public int shootCooldown = 20;
    private bool isOnCooldown = false;
    // Update is called once per frame
    void Update()
    {
        CheckDeath();
        CheckVelocity();
        UpdateLight();


        if (Input.GetMouseButtonDown(0) && !isOnCooldown)
        {
            Shoot();
            isOnCooldown = true;
        }
    }

    private void CheckKey()
    {
        if (hasKey)
        {
            SceneController.LoadWinScreen();
        }
        else
        {
            keyText.GetComponent<Animation>().Play("fade2");
        }
    }
    private void FixedUpdate()
    {
        if (isOnCooldown)
        {
            shootCooldown--;

            if (shootCooldown == 0)
            {
                shootCooldown = 20;
                isOnCooldown = false;
            }
        }
    }

    public void decreaseLife(float amount)
    {
        playerLight.intensity -= amount;
    }

    private void CheckVelocity()
    {
        Transform transform = gameObject.transform;
        Vector2 velocity = Vector2.zero;

        Vector3 pos = transform.position;
        velocity.x = Input.GetAxis("Horizontal");
        velocity.y = Input.GetAxis("Vertical");

        if (velocity.x >= walkThreshold || velocity.x <= -walkThreshold || 
            velocity.y >= walkThreshold || velocity.y <= -walkThreshold)
        {
            anim.SetBool("isWalking", true);
        } else
        {
            anim.SetBool("isWalking", false);
        }

        if (velocity.x < 0)
        {

            transform.eulerAngles = new Vector2(0, 180);
        }
        if (velocity.x > 0)
        {
            transform.eulerAngles = new Vector2(0, 0);
        }

        pos.x += velocity.x * Time.deltaTime * moveSpeed;
        pos.y += velocity.y * Time.deltaTime * moveSpeed;

        transform.position = Vector3.Lerp(transform.position, pos, lerpSpeed);
    }

    private void UpdateLight()
    {
        playerLight.intensity -= 0.0002f;
        UpdateHealth(playerLight.intensity);
    }

    private void UpdateHealth(float intensity)
    {
        if (intensity < 1 || intensity >= 0)
        {
            Color temp = healthBar.color;
            temp.a = intensity;
            healthBar.color = temp;
            healthBar2.color = temp;

            if (intensity <= .8)
            {
                temp = lightText1.color;
                temp.a = intensity;
                lightText1.color = temp;

                temp = lightText2.color;
                temp.a = intensity;
                lightText2.color = temp;
            }

            healthBar.fillAmount = intensity;
            healthBar2.fillAmount = intensity;
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.transform.tag == "Coin")
        {
            audioSource.PlayOneShot(pickup, .2f);
            CoinController coin = col.GetComponent<CoinController>();


            if (coin.isTriggerable)
            {
                IncreaseLife();
                col.transform.GetComponent<Animator>().Play("pickup");
                coin.isTriggerable = false;
            }
                
        }

        if (col.transform.tag == "Door")
        {
            CheckKey();
        }

        if (col.transform.tag == "Key")
        {
            hasKey = true;
            Destroy(col.gameObject);
        }
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.transform.tag == "Pushable" && col.transform.GetComponent<Pushable>().isPushable)
        {
            Vector3 temp = col.transform.position;
            temp.y += .05f;
            col.transform.position = temp;
        }
    }

    private void IncreaseLife()
    {
        float newLifeValue = playerLight.intensity + lifeIncrease;
        if (newLifeValue >= 1)
        {
            playerLight.intensity = 1;
        }   
        else
        {
            playerLight.intensity = newLifeValue;
        }
    }

    private int lastDeathCount = 0;
    private void CheckDeath()
    {

        if (SceneController.deathCount > lastDeathCount)
        {
            deathText.GetComponent<Animation>().Play("fade");
        }

        if (playerLight.intensity <= 0)
        {
            SceneController.LoadLevelOne();
            audioSource.PlayOneShot(die);
        }
        lastDeathCount = SceneController.deathCount;
    }

    public void Shoot()
    {

        Vector2 cursorWorldCoord = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector2 mouseVector = cursorWorldCoord - (Vector2) gameObject.transform.position;
        mouseVector.Normalize();

        Vector3 spawnLocation = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, -0.1f);
        GameObject bullet = (GameObject) Instantiate(bulletPrefab, spawnLocation, Quaternion.identity);
        Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(), bullet.GetComponent<Collider2D>());
        bullet.GetComponent<Rigidbody2D>().velocity = mouseVector * bulletSpeed;
    }
}
