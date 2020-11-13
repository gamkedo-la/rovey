using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    public RectTransform staminaBar;
    public PlayerStats playerStats;

    private void Update()
    {
        var staminaRatio = playerStats.stamina / playerStats.maxStamina;
        staminaBar.localScale = new Vector2(staminaRatio, 1f);
    }
}
