using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathTimer : MonoBehaviour
{
    public float TimeTillDead;

    // Update is called once per frame
    void Update()
    {
        if (TimeTillDead > 0)
        {
            TimeTillDead -= Time.deltaTime;
        }

        if (TimeTillDead < 0)
        {
            Destroy(gameObject);
        }
    }
}
