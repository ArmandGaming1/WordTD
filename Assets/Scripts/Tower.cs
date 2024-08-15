using UnityEngine;

public class Tower : MonoBehaviour
{
    [Header("Tower Settings")]
    public float range = 10f;            // Range within which the tower can target enemies
    public float fireRate = 1f;          // Time between each shot
    public int damage = 10;              // Damage dealt per shot
    public GameObject bulletPrefab;      // The bullet prefab to shoot
    public Transform firePoint;          // The point from which bullets are fired

    private float fireCountdown = 0f;    // Countdown to the next shot
    private Transform target;            // The current target enemy

    public bool isSelected = false;      // Flag to track if the tower is selected
    private Color originalColor;         // Store the original color of the tower

    void Start()
    {
        // Store the original color of the tower's material
        originalColor = GetComponent<Renderer>().material.color;
    }

    void Update()
    {
        // Find the nearest enemy in range
        UpdateTarget();

        if (target == null)
            return;

        // Rotate the tower to face the target
        Vector3 direction = target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        Vector3 rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * 5f).eulerAngles;
        transform.rotation = Quaternion.Euler(0f, rotation.y, 0f);

        if (fireCountdown <= 0f)
        {
            Shoot();
            fireCountdown = 1f / fireRate;
        }

        fireCountdown -= Time.deltaTime;
    }

    void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDistance && distanceToEnemy <= range)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }

        if (nearestEnemy != null && shortestDistance <= range)
        {
            target = nearestEnemy.transform;
        }
        else
        {
            target = null;
        }
    }

    void Shoot()
    {
        if (bulletPrefab != null && firePoint != null)
        {
            GameObject bulletGO = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            Bullet bullet = bulletGO.GetComponent<Bullet>();

            if (bullet != null)
            {
                bullet.Seek(target, damage);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        // Draw the tower's range in the editor
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }

    public void Select()
    {
        if (isSelected) return; // Avoid selecting the tower again if it's already selected

        isSelected = true;

        // Visual feedback for selection
        GetComponent<Renderer>().material.color = Color.green; // Change to green to indicate selection

        // Update the UI to reflect the selected tower's stats
        TowerSelectionManager.Instance.SelectTower(this);
    }

    public void Deselect()
    {
        if (!isSelected) return; // Avoid deselecting if the tower isn't selected

        isSelected = false;

        // Revert visual feedback to indicate the tower is no longer selected
        GetComponent<Renderer>().material.color = originalColor; // Revert to original color

        // Hide or update the UI to reflect that no tower is selected
        TowerSelectionManager.Instance.DeselectTower();
    }
}
