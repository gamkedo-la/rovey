using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShortcuts : MonoBehaviour
{
    public Transform[] warpLocations;
    private PlayerController controller;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<PlayerController>();
        if (!controller)
        {
            Debug.LogError("No PlayerController detected for use with PlayerShortcuts");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (controller && Input.GetKey(KeyCode.LeftShift))
        {
            var max = Mathf.Min(10, warpLocations.Length);
            for (int i = 0; i < max; i++)
            {
                KeyCode.TryParse<KeyCode>($"Alpha{i}", false, out var keycode);
                if (Input.GetKeyDown(keycode))
                {
                    controller.ResetPosition(warpLocations[i].position);
                }
            }
        }
    }
}
