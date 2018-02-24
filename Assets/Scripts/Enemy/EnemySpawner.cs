// Author(s): Paul Calande
// Class for spawning enemies in Rocket Puncher.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    [Tooltip("File to read score tuning from.")]
    TextAsset scoreFile;
    [SerializeField]
    [Tooltip("Reference to the Score instance.")]
    Score score;
    [SerializeField]
    [Tooltip("Reference to the PlayerPowerup instance.")]
    PlayerPowerup playerPowerup;
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

    Timer timerSpawn;
    Timer timerSpawnGroup;
    Timer timerDeadTime;

    // A collection of all the possible enemies that can spawn.
    List<Enemy.Data> possibleEnemies = new List<Enemy.Data>();
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

    // Read data from the files.
    private void Tune()
    {
        JSONNodeReader reader = new JSONNodeReader(enemySpawnersFile);
        challengeMax = reader.TryGetFloat("initial challenge", 1.0f);
        challengeIncreasePerSpawnGroup = reader.TryGetFloat("challenge increase per spawn group", 1.0f);
        spawnGroupsPerDeadTime = reader.TryGetInt("spawn groups per dead time", 3);
        secondsBetweenSpawns = reader.TryGetFloat("seconds between spawns", 1.0f);
        secondsBetweenSpawnGroups = reader.TryGetFloat("seconds between spawn groups", 2.0f);
        secondsPerDeadTime = reader.TryGetFloat("seconds per dead time", 3.0f);
        enemyBaseLeftMovementSpeed = reader.TryGetFloat("enemy base left movement speed", 0.05f);

        timerSpawn = new Timer(secondsBetweenSpawns);
        timerSpawnGroup = new Timer(secondsBetweenSpawnGroups);
        timerDeadTime = new Timer(secondsPerDeadTime);
        ChooseRandomSpawnPosition();

        // How much health a single health kit should give.
        int healthPerHealthKit;
        int pointsPerEnemyKilled;
        int pointsPerProjectilePunched;
        int pointsPerFullHealthHealthKit;

        reader.SetFile(scoreFile);
        pointsPerEnemyKilled = reader.TryGetInt("points per enemy killed", 100);
        pointsPerProjectilePunched = reader.TryGetInt("points per projectile punched", 10);
        pointsPerFullHealthHealthKit = reader.TryGetInt("points per full health health kit", 100);

        // Drop rates for items.
        Probability<ItemType> probItem = new Probability<ItemType>(ItemType.None);
        float dropRateHealthKit;
        float dropRateBattleAxe;
        float dropRateMoreArms;

        reader.SetFile(itemsFile);
        dropRateHealthKit = reader.TryGetFloat("health kit drop rate", 0.07f);
        dropRateBattleAxe = reader.TryGetFloat("battle axe drop rate", 0.05f);
        dropRateMoreArms = reader.TryGetFloat("more arms drop rate", 0.05f);
        probItem.SetChance(ItemType.HealthKit, dropRateHealthKit);
        probItem.SetChance(ItemType.BattleAxe, dropRateBattleAxe);
        probItem.SetChance(ItemType.MoreArms, dropRateMoreArms);
        healthPerHealthKit = reader.TryGetInt("health per health kit", 50);

        reader.SetFile(enemiesFile);
        JSONArrayReader enemyArray = reader.TryGetArray("enemies");
        //for (int i = 0; i < enemyArray.GetCount(); ++i)
        JSONNodeReader enemyNode;
        while (enemyArray.GetNextNode(out enemyNode))
        {
            //JSONNodeReader enemyNode = enemyArray.GetNode(i);
            string enemyName = enemyNode.TryGetString("name", "UNNAMED");

            // Read volley data.
            JSONNodeReader volleyNode = enemyNode.TryGetNode("volley");

            string colString = volleyNode.TryGetString("color", "#ffffff");
            Color projColor;
            if (!ColorUtility.TryParseHtmlString(colString, out projColor))
            {
                Debug.Log(enemyName + ": Could not parse HTML color for volley!");
            }
            EnemyProjectile.Data projectile = new EnemyProjectile.Data(
                volleyNode.TryGetBool("projectile punchable", true),
                volleyNode.TryGetInt("projectile damage", 20),
                pointsPerProjectilePunched,
                projColor,
                volleyNode.TryGetFloat("direction", 180.0f),
                volleyNode.TryGetFloat("speed", 4.0f),
                score);

            VolleyData volley = new VolleyData(projectile,
                volleyNode.TryGetInt("projectile count", 1),
                volleyNode.TryGetFloat("spread angle", 0.0f),
                volleyNode.TryGetBool("aims at player", false));

            OscillatePosition2D.Data oscData = new OscillatePosition2D.Data(0.0f, 0.0f,
                enemyNode.TryGetFloat("y oscillation magnitude", 0.0f),
                enemyNode.TryGetFloat("y oscillation speed", 0.0f));

            EnemyAttack.Data attack = new EnemyAttack.Data(
                new EnemyAttack.Data.Refs(playerPowerup.gameObject),
                volley,
                enemyNode.TryGetFloat("seconds between volleys", 1.0f),
                enemyNode.TryGetFloat("volley direction delta per shot", 0.0f),
                enemyBaseLeftMovementSpeed);

            EnemySprite.Data enemySprite = new EnemySprite.Data(
                enemyNode.TryGetString("sprite name", "basic"));

            LeftMovement.Data leftMovement = new LeftMovement.Data(
                enemyNode.TryGetFloat("left movement speed increase", 0.0f)
                + enemyBaseLeftMovementSpeed);

            ItemHealthKit.Data healthKitData = new ItemHealthKit.Data(
                new ItemHealthKit.Data.Refs(score),
                healthPerHealthKit,
                pointsPerFullHealthHealthKit);
            EnemyHealth.Data enemyHealthData = new EnemyHealth.Data(
                new EnemyHealth.Data.Refs(score, playerPowerup),
                healthKitData,
                pointsPerEnemyKilled,
                probItem);

            Enemy.Data enemy = new Enemy.Data(enemyNode.TryGetFloat("challenge", 1.0f),
                oscData,
                attack,
                enemySprite,
                leftMovement,
                enemyHealthData);

            // Add the enemy to the possible enemies pool.
            possibleEnemies.Add(enemy);
        }
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
        List<Enemy.Data> viableEnemies = GetViableEnemies();

        // Choose one of these enemies randomly.
        Enemy.Data enemy = viableEnemies[Random.Range(0, viableEnemies.Count)];
        challengeCurrent += enemy.challenge;

        // Instantiate the enemy.
        GameObject newEnemy = Instantiate(prefabEnemy, spawnPos, Quaternion.identity);
        Enemy en = newEnemy.GetComponent<Enemy>();
        en.SetData(enemy.DeepCopy());

        // Check if there are any viable enemies left.
        // If not, it's time to move on to the next spawn group.
        viableEnemies = GetViableEnemies();
        spawnGroupActive = (viableEnemies.Count != 0);
    }

    private List<Enemy.Data> GetViableEnemies()
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