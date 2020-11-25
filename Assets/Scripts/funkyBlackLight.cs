using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// this simple class inverts a light colour
// from white (or whatever) to black
// which is used to hack spotlights
// into acting like a "projector"
// that works in URP rendering mode
public class funkyBlackLight : MonoBehaviour
{
    void Start() {
       Debug.Log("Turning on a BLACK LIGHT (Rovey's drop shadow)");
       Light light = GetComponent<Light>();
       light.color = new Color(-1,-1,-1,1);
       //light.intensity = 50;
    }
}
