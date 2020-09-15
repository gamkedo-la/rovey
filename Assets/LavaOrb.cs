using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaOrb : MonoBehaviour
{
    public GameObject playerCollisionEffect;
    public GameObject groundCollisionEffect;

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.LogWarning("TODO: Deal damage to player");
            Despawn(playerCollisionEffect);
        }
        else
        {
            Despawn(groundCollisionEffect);
        }
    }

    private void Despawn(GameObject vfxPrefab)
    {
        var vfx = Instantiate(vfxPrefab, transform.position, Quaternion.identity);
        vfx.GetComponent<ParticleSystem>()?.Play();
        Destroy(gameObject);
    }
}
