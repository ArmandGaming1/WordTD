using System.Collections.Generic;
using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    public int width = 256;
    public int height = 256;
    public float heightMultiplier = 2f; // This will still be used for adjusting rock and tree height if needed

    public GameObject treePrefab;    // Reference to the tree prefab
    public int treeCount = 100;      // Number of trees to spawn
    public float maxTreeHeight = 1f; // Maximum height at which trees can be placed
    public float minDistanceBetweenTrees = 5f; // Minimum distance between trees

    public GameObject rockPrefab;    // Reference to the rock prefab
    public int rockCount = 50;       // Number of rocks to spawn
    public float maxRockHeight = 1f; // Maximum height at which rocks can be placed
    public float minDistanceBetweenRocks = 5f; // Minimum distance between rocks
    public float minDistanceBetweenRocksAndTrees = 7f; // Minimum distance between rocks and trees

    private List<Vector3> treePositions = new List<Vector3>();
    private List<Vector3> rockPositions = new List<Vector3>();

    void Start()
    {
        GenerateFlatTerrain();
        SpawnTrees();
        SpawnRocks();
    }

    void GenerateFlatTerrain()
    {
        Terrain terrain = GetComponent<Terrain>();
        TerrainData terrainData = terrain.terrainData;

        terrainData.heightmapResolution = width + 1;
        terrainData.size = new Vector3(width, heightMultiplier, width);

        // Generate a flat terrain with constant height
        float[,] heights = new float[width, width];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < width; y++)
            {
                heights[x, y] = 0; // Set all heights to 0 (flat terrain)
            }
        }

        terrainData.SetHeights(0, 0, heights);
    }

    void SpawnTrees()
    {
        Terrain terrain = GetComponent<Terrain>();
        if (treePrefab == null)
        {
            Debug.LogError("Tree Prefab not assigned!");
            return;
        }

        float terrainWidth = terrain.terrainData.size.x;
        float terrainHeight = terrain.terrainData.size.z;

        for (int i = 0; i < treeCount; i++)
        {
            Vector3 treePosition;
            bool positionFound = false;
            int attempts = 0;

            while (!positionFound && attempts < 100)
            {
                // Random position on the terrain
                float x = Random.Range(0, terrainWidth);
                float z = Random.Range(0, terrainHeight);

                // Get the terrain height at the (x, z) position (will be constant in this case)
                float y = 0; // Constant height for a flat terrain

                treePosition = new Vector3(x, y, z);

                // Check if the position is far enough from existing trees
                bool tooClose = false;
                foreach (Vector3 existingPosition in treePositions)
                {
                    if (Vector3.Distance(treePosition, existingPosition) < minDistanceBetweenTrees)
                    {
                        tooClose = true;
                        break;
                    }
                }

                // Ensure the position is not too close to existing rocks
                foreach (Vector3 existingRock in rockPositions)
                {
                    if (Vector3.Distance(treePosition, existingRock) < minDistanceBetweenRocksAndTrees)
                    {
                        tooClose = true;
                        break;
                    }
                }

                // Ensure the position is within the terrain bounds
                if (!tooClose && IsWithinTerrainBounds(treePosition, terrainWidth, terrainHeight))
                {
                    treePositions.Add(treePosition);
                    Instantiate(treePrefab, treePosition, Quaternion.identity);
                    positionFound = true;
                }
                else
                {
                    attempts++;
                }
            }
        }
    }

    void SpawnRocks()
    {
        Terrain terrain = GetComponent<Terrain>();
        if (rockPrefab == null)
        {
            Debug.LogError("Rock Prefab not assigned!");
            return;
        }

        float terrainWidth = terrain.terrainData.size.x;
        float terrainHeight = terrain.terrainData.size.z;

        for (int i = 0; i < rockCount; i++)
        {
            Vector3 rockPosition;
            bool positionFound = false;
            int attempts = 0;

            while (!positionFound && attempts < 100)
            {
                // Random position on the terrain
                float x = Random.Range(0, terrainWidth);
                float z = Random.Range(0, terrainHeight);

                // Get the terrain height at the (x, z) position (will be constant in this case)
                float y = 0; // Constant height for a flat terrain

                rockPosition = new Vector3(x, y, z);

                // Check if the position is far enough from existing rocks
                bool tooClose = false;
                foreach (Vector3 existingRock in rockPositions)
                {
                    if (Vector3.Distance(rockPosition, existingRock) < minDistanceBetweenRocks)
                    {
                        tooClose = true;
                        break;
                    }
                }

                // Ensure the position is not too close to existing trees
                foreach (Vector3 existingTree in treePositions)
                {
                    if (Vector3.Distance(rockPosition, existingTree) < minDistanceBetweenRocksAndTrees)
                    {
                        tooClose = true;
                        break;
                    }
                }

                // Ensure the position is within the terrain bounds
                if (!tooClose && IsWithinTerrainBounds(rockPosition, terrainWidth, terrainHeight))
                {
                    rockPositions.Add(rockPosition);
                    Instantiate(rockPrefab, rockPosition, Quaternion.identity);
                    positionFound = true;
                }
                else
                {
                    attempts++;
                }
            }
        }
    }

    bool IsWithinTerrainBounds(Vector3 position, float terrainWidth, float terrainHeight)
    {
        return position.x >= 0 && position.x <= terrainWidth &&
               position.z >= 0 && position.z <= terrainHeight;
    }
}
