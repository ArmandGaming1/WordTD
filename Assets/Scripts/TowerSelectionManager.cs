using UnityEngine;
using UnityEngine.UI;

public class TowerSelectionManager : MonoBehaviour
{
    public static TowerSelectionManager Instance;

    public GameObject selectionPanel; // The UI panel that appears when a tower is selected
    public Text towerStatsText;       // UI text component to show tower stats

    private Tower selectedTower;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        selectionPanel.SetActive(false); // Hide the panel initially
    }

    public void SelectTower(Tower tower)
    {
        if (selectedTower != null)
        {
            selectedTower.Deselect();
        }

        selectedTower = tower;
        UpdateUI();
    }

    void UpdateUI()
    {
        if (selectedTower != null)
        {
            selectionPanel.SetActive(true);
            towerStatsText.text = "Range: " + selectedTower.range + "\n" +
                                 "Fire Rate: " + selectedTower.fireRate + "\n" +
                                 "Damage: " + selectedTower.damage;
        }
    }

    public void DeselectTower()
    {
        selectedTower = null;
        selectionPanel.SetActive(false);
    }
}
