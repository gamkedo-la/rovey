using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    public RectTransform staminaBar;

    [Range(0f, 1.0f)] public float scaleSlider = 1.0f;

    private void Update()
    {
        staminaBar.localScale = new Vector2(scaleSlider, 1f);
    }
}
