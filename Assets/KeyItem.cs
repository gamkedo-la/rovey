using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class KeyItem : MonoBehaviour
{
    public float rotationSpeed = 60f;
    public float UIScale = 1f;
    public UnityEvent itemRetrieved;
    
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            itemRetrieved.Invoke();
        }
    }
}
