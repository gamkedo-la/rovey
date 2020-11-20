using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu]
public class PlayerStats : ScriptableObject
{
    public float maxStamina = 2.0f;
    public float stamina = 2.0f;
    public float jetpackStrength = 0.25f;
}
