using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    private Rigidbody enemyRb;
    private GameObject player;

    public float enemySpeed = 10f;

    float Ybound = -10;
    float Xbound = 20;
    float Zbound = 20;

    public bool isBoss;
    public float spawnInterval;
    private float nextSpawn;
    public int miniEnemySpawnCount;
    private SpawnManager spawnManager;

    // Start is called before the first frame update
    void Start()
    {
        enemyRb = GetComponent<Rigidbody>();
        player = GameObject.Find("Player");

        if (isBoss)
        {
            spawnManager = FindObjectOfType<SpawnManager>();
        }

    }

    // Update is called once per frame
    void Update()
    {
        //speed is fixed and every time the diff of vector increases then force applied increases
        //so to solve that we used normalized which returns magnitude of 1.
        Vector3 directionVector = (player.transform.position - transform.position).normalized;
        enemyRb.AddForce(directionVector * enemySpeed);

        if (isBoss)
        {
            if (Time.time > nextSpawn)
            {
                nextSpawn = Time.time + spawnInterval;
                spawnManager.SpawnMiniEnemy(miniEnemySpawnCount);
            }
        }
        if (transform.position.y < -10)
        {
            Destroy(gameObject);
        }

        //if enemy falls then destroy it
        if (transform.position.y < Ybound)
        {
            Destroy(gameObject);
        }
        else if(transform.position.z > Zbound || transform.position.z < -Zbound)
        {
            Destroy(gameObject);
        }
        else if(transform.position.x > Xbound || transform.position.x < -Xbound)
        {
            Destroy(gameObject);
        }
    }

    void bossMultiMinions()
    {
        
    }
}
