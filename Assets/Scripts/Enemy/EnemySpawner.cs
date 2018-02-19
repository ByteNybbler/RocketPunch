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
    [Tooltip("File containing item data.")]
    TextAsset itemsFile;
    [SerializeField]
    [Tooltip("Reference to the Score instance.")]
    Score score;
    [SerializeField]
    [Tooltip("The enemy prefab.")]
    GameObject prefabEnemy;
    [SerializeField]
    [Tooltip("The enemy spawn locations.")]
    GameObject[] spawns;

    // The current maximum challenge value for the spawn groups.
    float challengeMax;
    // How much the challenge value increases every time an enemy spawn group finishes.
    float challengeIncreasePerSpawnGroup;
    // How many spawn groups are required to begin the dead time.
    int spawnGroupsPerDeadTime;
    // How many seconds must pass between each spawn.
    float secondsBetweenSpawns;
    // How many seconds must pass between each spawn group (not during dead time).
    float secondsBetweenSpawnGroups;
    // How many seconds must pass during dead time.
    float secondsPerDeadTime;
    // How quickly all of the enemies move to the left.
    // Also affects how quickly enemy projectiles move to the left.
    // This creates the illusion that the player is moving to the right.
    float enemyBaseLeftMovementSpeed;
    // How much health a single health kit should give.
    int healthPerHealthKit;

    float dropRateHealthKit;
    float dropRateBattleAxe;
    float dropRateMoreArms;
    // Drop rates for items.
    Probability<ItemType> probItem = new Probability<ItemType>(ItemType.None);

    Timer timerSpawn;
    Timer timerSpawnGroup;
    Timer timerDeadTime;

    // A collection of all the possible enemies that can spawn.
    List<EnemyData> possibleEnemies = new List<EnemyData>();
    // How many spawn groups have occurred since the last dead time.
    int spawnGroupsSinceLastDeadTime = 0;
    // The current amount of challenge accumulated in this spawn group.
    float challengeCurrent = 0.0f;
    // The current spawn position to use to spawn new enemies.
    Vector3 spawnPos;
    // Whether the current spawn group has finished or not.
    bool spawnGroupActive = true;

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
        challengeMax = json["initial challenge"].AsFloat;
        challengeIncreasePerSpawnGroup = json["challenge increase per spawn group"].AsFloat;
        spawnGroupsPerDeadTime = json["spawn groups per dead time"].AsInt;
        secondsBetweenSpawns = json["seconds between spawns"].AsFloat;
        secondsBetweenSpawnGroups = json["seconds between spawn groups"].AsFloat;
        secondsPerDeadTime = json["seconds per dead time"].AsFloat;
        enemyBaseLeftMovementSpeed = json["enemy base left movement speed"].AsFloat;

        json = JSON.Parse(enemiesFile.ToString());
        JSONArray enemyArray = json["enemies"].AsArray;
        for (int i = 0; i < enemyArray.Count; ++i)
        {
            JSONNode enemyNode = enemyArray[i];
            string enemyName = UtilJSON.TryReadString(enemyNode["name"], "UNNAMED");

            // Read volley data.
            JSONNode volleyNode = enemyNode["volley"];

            string colString = UtilJSON.TryReadString(volleyNode["color"], "#ffffff");
            Color projColor;
            if (ColorUtility.TryParseHtmlString(colString, out projColor))
            {
                //Debug.Log(colString + " : " + volley.color);
            }
            else
            {
                Debug.Log(enemyName + ": Could not parse HTML color for volley!");
            }
            EnemyProjectile.Data projectile = new EnemyProjectile.Data(
                UtilJSON.TryReadBool(volleyNode["projectile punchable"], true),
                UtilJSON.TryReadInt(volleyNode["projectile damage"], 20),
                score.GetPointsPerProjectilePunched(),
                projColor,
                UtilJSON.TryReadFloat(volleyNode["direction"], 180.0f),
                UtilJSON.TryReadFloat(volleyNode["speed"], 4.0f),
                score);

            VolleyData volley = new VolleyData(projectile,
                UtilJSON.TryReadInt(volleyNode["projectile count"], 1),
                UtilJSON.TryReadFloat(volleyNode["spread angle"], 0.0f),
                UtilJSON.TryReadBool(volleyNode["aims at player"], false));

            OscillatePosition2D.Data oscData = new OscillatePosition2D.Data(0.0f, 0.0f,
                UtilJSON.TryReadFloat(enemyNode["y oscillation magnitude"], 0.0f),
                UtilJSON.TryReadFloat(enemyNode["y oscillation speed"], 0.0f));

            EnemyAttack.Data attack = new EnemyAttack.Data(volley,
                UtilJSON.TryReadFloat(enemyNode["seconds between volleys"], 1.0f),
                UtilJSON.TryReadFloat(enemyNode["volley direction delta per shot"], 0.0f),
                enemyBaseLeftMovementSpeed);

            EnemySprite.Data enemySprite = new EnemySprite.Data(enemyNode["sprite name"]);

            EnemyData enemy = new EnemyData(UtilJSON.TryReadFloat(enemyNode["challenge"], 1.0f),
                UtilJSON.TryReadFloat(enemyNode["left movement speed increase"], 0.0f),
                oscData,
                attack,
                enemySprite);

            // Add the enemy to the possible enemies pool.
            possibleEnemies.Add(enemy);
        }

        json = JSON.Parse(itemsFile.ToString());
        dropRateHealthKit = json["health kit drop rate"].AsFloat;
        dropRateBattleAxe = json["battle axe drop rate"].AsFloat;
        dropRateMoreArms = json["more arms drop rate"].AsFloat;
        probItem.SetChance(ItemType.HealthKit, dropRateHealthKit);
        probItem.SetChance(ItemType.BattleAxe, dropRateBattleAxe);
        probItem.SetChance(ItemType.MoreArms, dropRateMoreArms);
        healthPerHealthKit = json["health per health kit"].AsInt;
    }

    private void FixedUpdate()
    {
        if (spawnGroupsSinceLastDeadTime < spawnGroupsPerDeadTime)
        {
            if (spawnGroupActive)
            {
                while (timerSpawn.TimeUp(Time.deltaTime))
                {
                    ChooseRandomSpawnPosition();
                    SpawnEnemy();
                }
            }
            else
            {
                while (timerSpawnGroup.TimeUp(Time.deltaTime))
                {
                    challengeCurrent = 0.0f;
                    challengeMax += challengeIncreasePerSpawnGroup;
                    spawnGroupActive = true;
                    //ChooseRandomSpawnPosition();
                    spawnGroupsSinceLastDeadTime += 1;
                }
            }
        }
        else
        {
            while (timerDeadTime.TimeUp(Time.deltaTime))
            {
                spawnGroupsSinceLastDeadTime = 0;
            }
        }
    }

    // Choose a viable enemy based on the current level of challenge and spawn it.
    private void SpawnEnemy()
    {
        // Filter out all enemies that have too high of a challenge.
        List<EnemyData> viableEnemies = GetViableEnemies();

        // Choose one of these enemies randomly.
        EnemyData enemy = viableEnemies[Random.Range(0, viableEnemies.Count)];
        challengeCurrent += enemy.challenge;

        // Instantiate the enemy.
        GameObject obj = Instantiate(prefabEnemy, spawnPos, Quaternion.identity);

        EnemyAttack attack = obj.GetComponent<EnemyAttack>();
        attack.SetData(enemy.attackData);

        LeftMovement movement = obj.GetComponent<LeftMovement>();
        movement.SetMovementLeftSpeed(enemyBaseLeftMovementSpeed + enemy.leftMovementSpeedBonus);

        OscillatePosition2D oscillatePos = obj.GetComponent<OscillatePosition2D>();
        oscillatePos.SetData(enemy.oscData);

        EnemyHealth enemyHealth = obj.GetComponent<EnemyHealth>();
        ItemHealthKit.Data healthKitData = new ItemHealthKit.Data(healthPerHealthKit,
            score.GetPointsPerFullHealthHealthKit(),
            score);
        EnemyHealth.Data enemyHealthData = new EnemyHealth.Data(healthKitData,
            score.GetPointsPerEnemyKilled(),
            probItem,
            score);
        enemyHealth.SetData(enemyHealthData);

        EnemySprite enemySprite = obj.GetComponent<EnemySprite>();
        enemySprite.SetData(enemy.spriteData);

        // Check if there are any viable enemies left.
        // If not, it's time to move on to the next spawn group.
        viableEnemies = GetViableEnemies();
        spawnGroupActive = (viableEnemies.Count != 0);
    }

    private List<EnemyData> GetViableEnemies()
    {
        float challengeRemaining = challengeMax - challengeCurrent;
        return possibleEnemies.FindAll(x => x.challenge <= challengeRemaining);
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