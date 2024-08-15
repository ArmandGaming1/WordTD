using UnityEngine;

public class TargetSpawner : MonoBehaviour
{
    [Header("Spawn Area Settings")]
    public Vector3 spawnAreaMin; // The minimum coordinates for the spawn area
    public Vector3 spawnAreaMax; // The maximum coordinates for the spawn area
    private Terrain terrain; // Reference to the terrain

    [Header("Target Health Settings")]
    public int maxHP = 100; // Maximum HP of the target
    private int currentHP; // Current HP of the target

    void Start()
    {
        // Set the tag for the target
        gameObject.tag = "Target";

        // Get the terrain component
        terrain = Terrain.activeTerrain;

        if (terrain != null)
        {
            // Ensure the spawn area is within the terrain bounds
            AdjustSpawnAreaToTerrain();
            // Randomly position the target within the defined spawn area when the game starts
            RespawnTarget();
        }
        else
        {
            Debug.LogError("Terrain not found! Make sure there is an active terrain in the scene.");
        }

        // Initialize target health
        currentHP = maxHP;
    }

    void AdjustSpawnAreaToTerrain()
    {
        float terrainWidth = terrain.terrainData.size.x;
        float terrainHeight = terrain.terrainData.size.z;

        // Clamp the spawn area to be within the terrain bounds
        spawnAreaMin = new Vector3(
            Mathf.Clamp(spawnAreaMin.x, 0, terrainWidth),
            spawnAreaMin.y,
            Mathf.Clamp(spawnAreaMin.z, 0, terrainHeight)
        );

        spawnAreaMax = new Vector3(
            Mathf.Clamp(spawnAreaMax.x, 0, terrainWidth),
            spawnAreaMax.y,
            Mathf.Clamp(spawnAreaMax.z, 0, terrainHeight)
        );
    }

    public void RespawnTarget()
    {
        // Generate a random position within the defined spawn area
        Vector3 randomPosition = new Vector3(
            Random.Range(spawnAreaMin.x, spawnAreaMax.x),
            transform.position.y, // Keep the Y position fixed, or adjust if necessary
            Random.Range(spawnAreaMin.z, spawnAreaMax.z)
        );

        // Set the target's position to the randomly generated position
        transform.position = randomPosition;

        // Optional: Log the new position for debugging
        Debug.Log("Target spawned at: " + randomPosition);
    }

    // Method to deal damage to the target
    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        Debug.Log("Target took damage: " + damage + ", Current HP: " + currentHP);

        if (currentHP <= 0)
        {
            Die();
        }
    }

    // Method to get the current HP of the target
    public int GetCurrentHP()
    {
        return currentHP;
    }

    // Method to handle the target's destruction
    void Die()
    {
        Debug.Log("Target has been destroyed!");
        // Handle what happens when the target's HP reaches zero
        // Example: Trigger Game Over or restart the game
        GameOver();
        Destroy(gameObject); // Destroy the target
    }

    // Method to handle Game Over scenario
    void GameOver()
    {
        Debug.Log("Game Over!");
        // Implement the game over logic, such as stopping the game, showing a game over screen, etc.
        // Example: Stop the game or transition to a Game Over scene
        // Time.timeScale = 0f; // Pause the game
        // SceneManager.LoadScene("GameOverScene"); // Load a Game Over scene
    }
}
