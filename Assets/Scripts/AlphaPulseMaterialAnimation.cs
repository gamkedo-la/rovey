using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// this script changes the alpha of the object's material over time

public class AlphaPulseMaterialAnimation : MonoBehaviour
{
    public Material materialToFlicker;
    public float flickerSpeed = 10;
    public float flickerMin = 0f;
    public float flickerMax = 1f;

    void Update()
    {
        if (!materialToFlicker) return;
        Color tempcolor = materialToFlicker.color;
        tempcolor.a = flickerMin + Mathf.Abs(Mathf.Cos(Time.time/flickerSpeed))*(flickerMax-flickerMin);
        materialToFlicker.color = tempcolor; // hmm this seems to do nothing
        Debug.Log("AlphaPulseMaterialAnimation alpha:"+tempcolor.a.ToString("F2")); // values look right
    }
}
