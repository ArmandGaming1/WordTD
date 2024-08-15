using System.Collections.Generic;
using UnityEngine;

public class PathGenerator : MonoBehaviour
{
    public Transform target;
    public int numberOfWaypoints = 10;
    public float pathWidth = 5f;
    public float minDistanceBetweenWaypoints = 5f;

    public List<Vector3> GenerateRandomPath(Vector3 startPoint)
    {
        List<Vector3> waypoints = new List<Vector3>();
        waypoints.Add(startPoint);

        Vector3 currentPoint = startPoint;

        for (int i = 0; i < numberOfWaypoints - 1; i++)
        {
            Vector3 nextPoint = GenerateRandomWaypoint(currentPoint, target.position);
            waypoints.Add(nextPoint);
            currentPoint = nextPoint;
        }

        // Add the final point (the target)
        waypoints.Add(target.position);

        // Optionally visualize the path for debugging
        for (int i = 0; i < waypoints.Count - 1; i++)
        {
            Debug.DrawLine(waypoints[i], waypoints[i + 1], Color.red, 5f);
        }

        return waypoints;
    }

    private Vector3 GenerateRandomWaypoint(Vector3 currentPoint, Vector3 targetPoint)
    {
        Vector3 directionToTarget = (targetPoint - currentPoint).normalized;
        Vector3 randomOffset = new Vector3(
            Random.Range(-pathWidth, pathWidth),
            0, // Assuming a flat terrain; adjust Y if needed
            Random.Range(-pathWidth, pathWidth)
        );

        Vector3 nextPoint = currentPoint + directionToTarget * Random.Range(minDistanceBetweenWaypoints, minDistanceBetweenWaypoints * 2) + randomOffset;

        return nextPoint;
    }
}
