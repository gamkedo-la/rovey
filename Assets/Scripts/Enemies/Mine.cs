using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour
{
    /// <summary>
    /// Just a simple script to make the mine see the player when it gets close and triggers the explosion sequence, which is entirely controlled by the animator.
    /// By Xist3nce/Trent - Ping me on the slack if you have any questions about any of my scripts.
    /// </summary>

    [SerializeField]//I do this to be able to mess with the variable in the inspector but not exposing it to any other scripts ever.
    private Animator Anim;

    [SerializeField]
    private GameObject explosionPrefab;


    void OnTriggerEnter(Collider col) //When the trigger gets a collision it checks for a player, then executes startexplodesequence.
    {
        if (col.tag == "Player")
        {
            Anim.SetTrigger("StartExploding");
        }
        
    }


    public void Explode()
    {
        //Make a boom and maybe some particles etc.
        SpawnExplosionVFX();
        Debug.Log("Boom");
        Die();
    }
    private void SpawnExplosionVFX()
    {
        GameObject exp = Instantiate(explosionPrefab);  //spawns the explosion prefab we set in the inspector
        exp.transform.SetParent(null); //makes sure the explosion has no parent so when the mine dies it doesn't kill the particles
        ParticleSystem ps = exp.GetComponent<ParticleSystem>(); //Makes a local particle system variable named ps that I then set to the explosion instances' particle system.
        ps.Play(); //Just a backup to make sure the explosion plays, for some reason without it it had start lag.
        

    }
    private void Die() //Destroys the game object.
    {
        Destroy(gameObject);
    }
}
