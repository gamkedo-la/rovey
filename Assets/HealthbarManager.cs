using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthbarManager : MonoBehaviour
{
    public GameObject healthIndicatorWidget;
    public int playerMaxHealth = 5;
    private int playerCurrentHealth;

    private List<GameObject> widgets;

    // Start is called before the first frame update
    void Start()
    {
        playerCurrentHealth = playerMaxHealth;
        AddHealth(playerMaxHealth);
    }

    public void AddHealth(int amount = 1)
    {
        for (int i = 0; i < amount; i++)
        {
            Instantiate(healthIndicatorWidget, Vector3.zero, Quaternion.identity, transform);
        }
    }

    public void RemoveHealth(int amount = 1)
    {
        var targetHealth = playerCurrentHealth - amount;
        while (transform.childCount > targetHealth)
        {
            Destroy(transform.GetChild(0));
        }
    }
}
