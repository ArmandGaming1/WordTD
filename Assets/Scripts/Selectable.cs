using UnityEngine;

public class Selectable : MonoBehaviour
{
    private Tower tower;

    void Start()
    {
        tower = GetComponent<Tower>();
    }

    void OnMouseDown()
    {
        // Handle tower selection logic
        if (tower != null)
        {
            tower.Select();
        }
    }
}
