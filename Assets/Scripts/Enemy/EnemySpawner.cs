// Author(s): Paul Calande
// Class for spawning enemies in Rocket Puncher.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    [Tooltip("File containing enemy data.")]
    TextAsset enemiesFile;
    [SerializeField]
    [Tooltip("File containing enemy spawner data.")]
    TextAsset enemySpawnersFile;
    [SerializeField]
    [Tooltip("The enemy prefab.")]
    GameObject prefabEnemy;
    [SerializeField]
    [Tooltip("The enemy spawn locations.")]
    GameObject[] spawns;

    // The current maximum challenge value for the spawn groups.
    int challengeMax;
    // How much the challenge value increases every time an enemy spawn group finishes.
    int challengeIncreasePerSpawnGroup;
    // How many spawns are required to begin the dead time.
    int spawnsPerDeadTime;
    // How many seconds must pass between each spawn.
    float secondsBetweenSpawns;
    // How many seconds must pass between each spawn group (not during dead time).
    float secondsBetweenSpawnGroups;
    // How many seconds must pass during dead time.
    float secondsPerDeadTime;

    Timer timerSpawn;
    Timer timerSpawnGroup;
    Timer timerDeadTime;

    // A collection of all the possible enemies that can spawn.
    List<EnemyData> possibleEnemies = new List<EnemyData>();
    // How many spawns have occurred since the last dead time.
    int spawnsSinceLastDeadTime = 0;
    // The current amount of challenge accumulated in this spawn group.
    int challengeCurrent = 0;
    // The current spawn position to use to spawn new enemies.
    Vector3 spawnPos;

    private void Awake()
    {
        Tune();
    }

    private void Start()
    {
        timerSpawn = new Timer(secondsBetweenSpawns);
        timerSpawnGroup = new Timer(secondsBetweenSpawnGroups);
        timerDeadTime = new Timer(secondsPerDeadTime);
        ChooseRandomSpawnPosition();
    }

    // Read data from the files.
    private void Tune()
    {
        JSONNode json = JSON.Parse(enemySpawnersFile.ToString());
        challengeMax = json["initial challenge"].AsInt;
        challengeIncreasePerSpawnGroup = json["challenge increase per spawn group"].AsInt;
        spawnsPerDeadTime = json["spawns per dead time"].AsInt;
        secondsBetweenSpawns = json["seconds between spawns"].AsFloat;
        secondsBetweenSpawnGroups = json["seconds between spawn groups"].AsFloat;
        secondsPerDeadTime = json["seconds per dead time"].AsFloat;

        json = JSON.Parse(enemiesFile.ToString());
        JSONArray enemyArray = json["enemies"].AsArray;
        for (int i = 0; i < enemyArray.Count; ++i)
        {
            JSONNode enemyNode = enemyArray[i];
            string enemyName = UtilJSON.TryReadString(enemyNode["name"], "UNNAMED");
            int challenge = UtilJSON.TryReadInt(enemyNode["challenge"], 1);
            EnemyData enemy = new EnemyData(challenge);
            enemy.leftMovementSpeed = UtilJSON.TryReadFloat(enemyNode["left movement speed"], 0.05f);
            enemy.yOscillationMagnitude = UtilJSON.TryReadFloat(enemyNode["y oscillation magnitude"], 0.0f);
            enemy.yOscillationSpeed = UtilJSON.TryReadFloat(enemyNode["y oscillation speed"], 0.0f);
            enemy.secondsBetweenVolleys = UtilJSON.TryReadFloat(enemyNode["seconds between volleys"], 1.0f);
            enemy.volleyDirectionDeltaPerShot = UtilJSON.TryReadFloat(enemyNode["volley direction delta per shot"], 0.0f);
            
            // Read volley data.
            JSONNode volleyNode = enemyNode["volley"];
            VolleyData volley = new VolleyData();
            volley.speed = UtilJSON.TryReadFloat(volleyNode["speed"], 4.0f);
            volley.direction = UtilJSON.TryReadFloat(volleyNode["direction"], 180.0f);
            volley.projectileCount = UtilJSON.TryReadInt(volleyNode["projectile count"], 1);
            volley.spreadAngle = UtilJSON.TryReadFloat(volleyNode["spread angle"], 0.0f);
            volley.projectilePunchable = UtilJSON.TryReadBool(volleyNode["projectile punchable"], true);
            volley.aimAtPlayer = UtilJSON.TryReadBool(volleyNode["aims at player"], false);
            string colString = UtilJSON.TryReadString(volleyNode["color"], "#ffffff");
            if (ColorUtility.TryParseHtmlString(colString, out volley.color))
            {
                //Debug.Log(colString + " : " + volley.color);
            }
            else
            {
                Debug.Log(enemyName + ": Could not parse HTML color for volley!");
            }

            enemy.volley = volley;
            // Add the enemy to the possible enemies pool.
            possibleEnemies.Add(enemy);
        }
    }

    private void FixedUpdate()
    {
        if (spawnsSinceLastDeadTime < spawnsPerDeadTime)
        {
            if (challengeCurrent < challengeMax)
            {
                while (timerSpawn.TimeUp(Time.deltaTime))
                {
                    SpawnEnemy();
                }
            }
            else
            {
                while (timerSpawnGroup.TimeUp(Time.deltaTime))
                {
                    challengeCurrent = 0;
                    challengeMax += challengeIncreasePerSpawnGroup;
                    ChooseRandomSpawnPosition();
                }
            }
        }
        else
        {
            while (timerDeadTime.TimeUp(Time.deltaTime))
            {
                spawnsSinceLastDeadTime = 0;
            }
        }
    }

    // Choose a viable enemy based on the current level of challenge and spawn it.
    private void SpawnEnemy()
    {
        int challengeRemaining = challengeMax - challengeCurrent;
        // Filter out all enemies that have too high of a challenge.
        List<EnemyData> viableEnemies =
            possibleEnemies.FindAll(x => x.GetChallenge() <= challengeRemaining);

        // Choose one of these enemies randomly.
        EnemyData enemy = viableEnemies[Random.Range(0, viableEnemies.Count)];
        challengeCurrent += enemy.GetChallenge();

        // Instantiate the enemy.
        GameObject obj = Instantiate(prefabEnemy, spawnPos, Quaternion.identity);

        EnemyAttack attack = obj.GetComponent<EnemyAttack>();
        attack.Init(enemy.volley, enemy.secondsBetweenVolleys, enemy.volleyDirectionDeltaPerShot);

        EnemyMovement movement = obj.GetComponent<EnemyMovement>();
        movement.SetMovementLeftSpeed(enemy.leftMovementSpeed);

        OscillatePosition2D oscillatePos = obj.GetComponent<OscillatePosition2D>();
        oscillatePos.Init(0.0f, 0.0f, enemy.yOscillationMagnitude, enemy.yOscillationSpeed);
    }

    private Vector3 GetRandomSpawnPosition()
    {
        return spawns[Random.Range(0, spawns.Length)].transform.position;
    }

    private void ChooseRandomSpawnPosition()
    {
        spawnPos = GetRandomSpawnPosition();
    }
}