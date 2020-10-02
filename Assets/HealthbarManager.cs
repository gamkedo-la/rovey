using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthbarManager : MonoBehaviour
{
    public GameObject healthIndicatorWidget;
    public int playerMaxHealth = 5;
    public Color lowColor;
    public Color highColor;
    public Color extraColor;

    private List<GameObject> widgets;
    private Color[] colors;

    // Start is called before the first frame update
    void Start()
    {
        colors = new Color[5];
        SetupColors();

        RemoveHealth(transform.childCount);
        AddHealth(playerMaxHealth);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftBracket))
        {
            RemoveHealth();
        }
        else if (Input.GetKeyDown(KeyCode.RightBracket))
        {
            AddHealth();
        }
    }

    private void SetupColors()
    {
        Color.RGBToHSV(lowColor, out float lHue, out float lSaturation, out float lValue);
        Color.RGBToHSV(highColor, out float hHue, out _, out _);
        var stepSize = (hHue - lHue) / (playerMaxHealth - 1);

        for (var i = 0; i < playerMaxHealth; i++)
        {
            var hue = lHue + (stepSize * i);
            colors[i] = Color.HSVToRGB(hue, lSaturation, lValue);
        }
    }

    public void AddHealth(int amount = 1)
    {
        for (var i = 0; i < amount; i++)
        {
            var ind = Instantiate(healthIndicatorWidget, Vector3.zero, Quaternion.identity, transform);
            var colorIndex = transform.childCount - 1;
            var color = (colorIndex > colors.Length - 1) ? extraColor : colors[colorIndex];
            ind.GetComponent<CanvasRenderer>()?.SetColor(color);
        }
    }

    public void RemoveHealth(int amount = 1)
    {
        if (transform.childCount == 0) return;

        var childCount = transform.childCount;
        var amountToRemove = (childCount < amount) ? childCount : amount;

        for (var i = 0; i < amountToRemove; i++)
        {
            var child = transform.GetChild(transform.childCount - 1);
            Destroy(child.gameObject);
        }
    }
}
