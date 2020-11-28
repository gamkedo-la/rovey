using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public string nextLevel;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Check if all 3 items have been collected, transition to the next level.
            var playerController = other.GetComponent<PlayerController>();
            if (playerController.KeyItems >= 3)
            {
                SceneManager.LoadScene(nextLevel);
            }
        }
    }
}
