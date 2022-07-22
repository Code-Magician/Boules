using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public PowerUpType currentPowerUp = PowerUpType.None;

    private Rigidbody playerRb;
    private GameObject cameraFocalPoint;
    public GameObject powerIndicator;
    public GameObject bulletPrefab;
    private GameObject tmpRocket;
    private Coroutine powerupCountdown;

    private Vector3 powerIndictorOffset = new Vector3(0, 0.5f, 0);
    private float powerUpIndicatorRotationSpeed = 10f;

    public float playerSpeed;

    public bool hasPowerUp = false;

    float powerupTime = 5f;

    private float powerUpForce = 100f;

    public float hangTime;
    public float smashSpeed;
    public float explosionForce;
    public float explosionRadius;
    bool smashing = false;
    float floorY;


    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        cameraFocalPoint = GameObject.Find("CameraFocalPoint");
    }

    // Update is called once per frame
    void Update()
    {
        movePlayer();

        // position of powerindicator = player position..
        powerIndicator.transform.position = transform.position - powerIndictorOffset;

        //adding functionality to powerup and powerupindicator
        powerUpIndicatorCool();

        if (currentPowerUp == PowerUpType.Bullets && Input.GetKeyDown(KeyCode.F))
        {
            LaunchRockets();
        }

        if (currentPowerUp == PowerUpType.Smash && Input.GetKeyDown(KeyCode.Space) && !smashing)
        {
            smashing = true;
            StartCoroutine(Smash());
        }

    }


    //moving player....
    void movePlayer()
    {
        //bcoz hume w and s keys use krni hain...
        float verticalInput = Input.GetAxis("Vertical");
        float horizontalInput = Input.GetAxis("Horizontal");

        playerRb.AddForce(cameraFocalPoint.transform.forward * playerSpeed * verticalInput * Time.deltaTime, ForceMode.Impulse);
        playerRb.AddForce(cameraFocalPoint.transform.right * playerSpeed * horizontalInput * Time.deltaTime, ForceMode.Impulse);

    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PowerUp"))
        {
            hasPowerUp = true;
            currentPowerUp = other.gameObject.GetComponent<PowerUp>().powerupType;

            Destroy(other.gameObject);

            powerIndicator.SetActive(true);
            if(powerupCountdown != null)
            {
                StartCoroutine(timerPowerUp());
            }
            //timer after which hasPowerUp will become false
            powerupCountdown =  StartCoroutine(timerPowerUp());
        }
        
    }


    //when colliding with enemy
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && currentPowerUp == PowerUpType.SuperPower)
        {
            Rigidbody enemyRb = collision.gameObject.GetComponent<Rigidbody>();
            // direction in which enemy goes is (enemydirecction - playerdirection)
            Vector3 direction = (collision.gameObject.transform.position - gameObject.transform.position).normalized;

            enemyRb.AddForce(direction * powerUpForce, ForceMode.Impulse);
        }
    }

    void LaunchRockets()
    {
        foreach (FollowPlayer enemy in FindObjectsOfType<FollowPlayer>())
        {
            tmpRocket = Instantiate(bulletPrefab, transform.position + Vector3.up,
            Quaternion.identity);
            tmpRocket.GetComponent<BulletBehaviour>().Fire(enemy.gameObject.transform);
        }
    }


    // funtion to time for which powerup will remain....
    IEnumerator timerPowerUp()
    {
        yield return new WaitForSeconds(powerupTime);
        hasPowerUp = false;
        currentPowerUp = PowerUpType.None;
        powerIndicator.SetActive(false);
    }


    //making powerupIndicator cool
    void powerUpIndicatorCool()
    {
        powerIndicator.gameObject.transform.Rotate(Vector3.up, powerUpIndicatorRotationSpeed);
    }


    IEnumerator Smash()
    {
        FollowPlayer[] enemies = FindObjectsOfType<FollowPlayer>();
        //Store the y position before taking off
        floorY = transform.position.y;
        //Calculate the amount of time we will go up
        float jumpTime = Time.time + hangTime;
        while (Time.time < jumpTime)
        {
            //move the player up while still keeping their x velocity.
            playerRb.velocity = new Vector2(playerRb.velocity.x, smashSpeed);
            yield return null;
        }
        //Now move the player down
        while (transform.position.y > floorY)
        {
            playerRb.velocity = new Vector2(playerRb.velocity.x, -smashSpeed * 2);
            yield return null;
        }
        //Cycle through all enemies.
        for (int i = 0; i < enemies.Length; i++)
        {
            //Apply an explosion force that originates from our position.
            if (enemies[i] != null)
                enemies[i].gameObject.GetComponent<Rigidbody>().AddExplosionForce(explosionForce,
                transform.position, explosionRadius, 0.0f, ForceMode.Impulse);
        }
        //We are no longer smashing, so set the boolean to false
        smashing = false;
    }


}
