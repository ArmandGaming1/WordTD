using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Spawner Settings")]
    public GameObject enemyPrefab;       // The enemy prefab to spawn
    public PathGenerator pathGenerator;  // Reference to the PathGenerator in the scene
    public float spawnInterval = 5f;     // Time between enemy spawns
    public List<Transform> spawnPoints;  // List of possible spawn points

    [Header("Enemy Settings")]
    public float enemySpeed = 5f;        // Speed of the enemies
    public int maxHP = 20;               // Maximum HP of the enemy
    private int currentHP;               // Current HP of the enemy
    public int damageToTarget = 10;      // Damage dealt to the target when reached

    void Start()
    {
        // Initialize enemy health
        currentHP = maxHP;

        // Start spawning enemies at regular intervals
        InvokeRepeating("SpawnEnemy", 0f, spawnInterval);
    }

    void SpawnEnemy()
    {
        if (enemyPrefab != null && pathGenerator != null && spawnPoints.Count > 0)
        {
            // Select a random spawn point from the list
            Transform selectedSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];

            // Generate a new path for each enemy starting from the selected spawn point
            Vector3 spawnPoint = selectedSpawnPoint.position;
            List<Vector3> waypoints = pathGenerator.GenerateRandomPath(spawnPoint);

            // Instantiate the enemy at the selected spawn point
            GameObject enemy = Instantiate(enemyPrefab, spawnPoint, Quaternion.identity);

            // Start the movement coroutine for this enemy
            StartCoroutine(MoveAlongPath(enemy, waypoints));
        }
        else
        {
            Debug.LogError("EnemyPrefab, PathGenerator, or SpawnPoints not properly assigned!");
        }
    }

    private IEnumerator MoveAlongPath(GameObject enemy, List<Vector3> waypoints)
    {
        int currentWaypointIndex = 0;

        while (currentWaypointIndex < waypoints.Count)
        {
            Vector3 targetWaypoint = waypoints[currentWaypointIndex];
            Vector3 direction = (targetWaypoint - enemy.transform.position).normalized;

            while (Vector3.Distance(enemy.transform.position, targetWaypoint) > 0.5f)
            {
                enemy.transform.position += direction * enemySpeed * Time.deltaTime;
                yield return null;
            }

            currentWaypointIndex++;
        }

        // Enemy reached the final waypoint, which is the target
        if (enemy != null) // Ensure the enemy still exists
        {
            DealDamageToTarget();
            Destroy(enemy); // Destroy the enemy GameObject
        }
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        Debug.Log("Enemy took damage: " + damage + ", Current HP: " + currentHP);

        if (currentHP <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Enemy has been destroyed!");
        // Implement additional logic if needed before destroying the enemy
        Destroy(gameObject);
    }

    void DealDamageToTarget()
    {
        // Assuming the last waypoint is the target position
        GameObject target = GameObject.FindGameObjectWithTag("Target");
        if (target != null)
        {
            TargetSpawner targetSpawner = target.GetComponent<TargetSpawner>();
            if (targetSpawner != null)
            {
                targetSpawner.TakeDamage(damageToTarget);

                // Check if the target's HP is zero after taking damage
                if (targetSpawner.GetCurrentHP() <= 0)
                {
                    GameOver();
                }
            }
        }
    }

    void GameOver()
    {
        // Handle the Game Over scenario
        Debug.Log("Game Over!");
        // Here you could stop the game, show a Game Over screen, etc.
        // Example: Stop spawning enemies
        CancelInvoke("SpawnEnemy");
    }
}
