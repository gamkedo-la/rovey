using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ThirdPersonCamera : MonoBehaviour
{
    // Player object to follow
    public Transform player;
    private Vector3 pan = Vector3.zero;

    // Control how quickly the camera catches up with player
    // movement and rotation
    public float translationDamping = 5f;
    public float rotationDamping = 5f;
    
    // Camera in hierarchy
    private Transform camera;
    
    void Start()
    {
        camera = gameObject.GetComponentInChildren<Camera>().transform;
    }

    private void LateUpdate()
    {
        // Catch up with the player
        transform.position = Vector3.Lerp(transform.position, player.position, Time.deltaTime * translationDamping);
        
        // Look in the same direction as the player
        pan.y = player.rotation.eulerAngles.y;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(pan), Time.deltaTime * rotationDamping);
    }
}
