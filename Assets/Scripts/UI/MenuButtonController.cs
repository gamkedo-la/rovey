using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButtonController : MonoBehaviour
{
    //From Thomas Brush tutorial https://www.youtube.com/watch?v=vqZjZ6yv1lA


    public int index;
    [SerializeField] bool keyDown;
    [SerializeField] int maxIndex;

    public AudioSource audioSource;


    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetAxis("Vertical") != 0)
        {
            if (!keyDown)
            {
                if(Input.GetAxis("Vertical") < 0)
                {
                    if(index < maxIndex)
                    {
                        index++;
                    }
                    else
                    {
                        index = 0;
                    }
                }else if(Input.GetAxis("Vertical") > 0)
                {
                    if(index > 0)
                    {
                        index--;
                    }
                    else
                    {
                        index = maxIndex;
                    }
                }
                keyDown = true;
            }
        }
        else
        {
            keyDown = false;
        }
    }
}
