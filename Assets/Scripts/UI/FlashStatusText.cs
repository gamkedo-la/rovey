using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FlashStatusText : MonoBehaviour
{
    public TextMeshProUGUI text;
    public float flashRate = 0.1f;
    public float messageDuration = 2.0f;

    private float messageShownAtTime;

    // Start is called before the first frame update
    void Start()
    {
        if (!text)
        {
            text = GetComponent<TextMeshProUGUI>();
            text.enabled = false;
        }
    }

    private void Update()
    {
        if (text.enabled)
        {
            var timePassed = Time.time - messageShownAtTime;
            if (timePassed > messageDuration)
            {
                text.text = "";
                text.enabled = false;
            }
        }
    }

    public void ShowMessage(string message)
    {
        text.enabled = true;
        text.text = message;
        messageShownAtTime = Time.time;
    }
}
