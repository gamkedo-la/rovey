using System.Collections;
using System.Collections.Generic;
using UnityEngine;



//From Thomas Brush tutorial https://www.youtube.com/watch?v=vqZjZ6yv1lA

public class AnimatorFunctions : MonoBehaviour
{
    [SerializeField] MenuButtonController menuButtonController;
    public bool disableOnce;
    [SerializeField] LoadScenes loadScenes;

    void PlaySound(AudioClip audioClip)
    {
        if (!disableOnce)
        {
            menuButtonController.audioSource.PlayOneShot(audioClip);
        }
        else
        {
            disableOnce = false;
        }
    }

}
