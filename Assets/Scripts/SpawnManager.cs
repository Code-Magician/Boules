using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] enemyPrefab;
    public GameObject[] powerUpPrefab;
    public GameObject[] bossPrefab;

    private float spawnRange = 9f;

    private int enemyCount;
    private int level = 1;

    int maxEnemies = 10;

    public GameObject[] miniEnemyPrefabs;
    public int bossRound = 5;

    // Start is called before the first frame update
    void Start()
    {
        spawnEnemyWave(level);
        spawnPowerUps();
    }

    // Update is called once per frame
    void Update()
    {
        //increasing level
        increaseLevel();

       

    }

    void spawnEnemyWave(int enemysToSpawn)
    {
            for (int i = 0; i < enemysToSpawn; i++)
            {
                int randIndex = Random.Range(0, enemyPrefab.Length);
                //generate enemy
                Instantiate(enemyPrefab[randIndex], generateRandomLocation(), enemyPrefab[randIndex].transform.rotation);
            }
        

    }

    void spawnPowerUps()
    {
        int powerUpsToSpawn = Random.Range(1, 3);

        for (int i = 0; i < powerUpsToSpawn; i++)
        {
            int randIndex = Random.Range(0, powerUpPrefab.Length);
            // generate powerups
            Instantiate(powerUpPrefab[randIndex], generateRandomLocation(), Quaternion.identity);
        }
    }


    //increasing level of game
    void increaseLevel()
    {
        // find all the objects which have followplayer script
        enemyCount = FindObjectsOfType<FollowPlayer>().Length;

        if (enemyCount == 0)
        {
            destroyOldPowerUps();

            level++;
            int nOfEnemy = level;

            //enemies not more than 12 
            if (nOfEnemy >= maxEnemies)
                nOfEnemy = maxEnemies;

            if (level % bossRound != 0)
                spawnEnemyWave(nOfEnemy);
            else
                SpawnBossWave(level);
            spawnPowerUps();
        }
    }

    // random location
    Vector3 generateRandomLocation()
    {
        float randomSpawnX = Random.Range(-spawnRange, spawnRange);
        float randomSpawnZ = Random.Range(-spawnRange, spawnRange);

        Vector3 spawnLocation = new Vector3(randomSpawnX, 0, randomSpawnZ);

        return spawnLocation;
    }

    //destroy old levels powerups...
    void destroyOldPowerUps()
    {
        PowerUp[] oldPowerUps = GameObject.FindObjectsOfType<PowerUp>();

        foreach(PowerUp x in oldPowerUps)
        {
            Destroy(x.gameObject);
        }
    }

    void SpawnBossWave(int currentRound)
    {
        int miniEnemysToSpawn;
        //We dont want to divide by 0!
        if (bossRound != 0)
        {
            miniEnemysToSpawn = currentRound / bossRound;
        }
        else
        {
            miniEnemysToSpawn = 1;
        }
        var boss = Instantiate(bossPrefab[0], generateRandomLocation(), bossPrefab[0].transform.rotation);
        boss.GetComponent<FollowPlayer>().miniEnemySpawnCount = miniEnemysToSpawn;
    }


    public void SpawnMiniEnemy(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            int randomMini = Random.Range(0, miniEnemyPrefabs.Length);
            Instantiate(miniEnemyPrefabs[randomMini], generateRandomLocation(),
            miniEnemyPrefabs[randomMini].transform.rotation);
        }
    }


}
