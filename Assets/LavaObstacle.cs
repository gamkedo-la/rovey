using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaObstacle : MonoBehaviour
{
    public GameObject prefab;

    // Time between waves of obstacles.
    public float waveDelay = 2.0f;
    // Time between individual obstacles being launched.
    public float spawnDelay = 0.33f;
    // Number of obstacles in a wave.
    public int waveSize = 3;
    private int currentWaveRemaining;

    // Force strength of obstacle launch. Launches based on the up direction
    // from the LavaObstacle instance.
    public float launchStrength = 10.0f;

    // Start is called before the first frame update
    void Start()
    {
        currentWaveRemaining = waveSize;
        StartCoroutine(LaunchObstacle());
    }

    IEnumerator LaunchObstacle()
    {
        while (currentWaveRemaining > 0)
        {
            var obstacle = Instantiate(prefab, transform);
            obstacle.GetComponent<Rigidbody>().AddForce(transform.up * launchStrength, ForceMode.Impulse);
            currentWaveRemaining--;
            yield return new WaitForSeconds(spawnDelay);
        }

        StartCoroutine(WaveCooldown());
    }

    IEnumerator WaveCooldown()
    {
        currentWaveRemaining = waveSize;
        yield return new WaitForSeconds(waveDelay);
        StartCoroutine(LaunchObstacle());
    }

}
