using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
	public AudioClip[] jump;
    public float jumpVolume = 1f;
	public AudioClip[] land;
    public float landVolume = 0.5f;
	public AudioClip[] step;
    public float stepVolume = 0.5f;
	public AudioClip jetpack;

	private AudioSource jetpackSource;
	private Animator animator;

    private bool errorDetected = false;

	 void Start()
	{
		animator = GetComponent<Animator>();
	}

	void StartJump()
	{
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySoundSFX(jump[Random.Range(0, jump.Length)], gameObject, jumpVolume * Random.Range(0.8f, 1f), Random.Range(0.8f, 1.2f));
        }
        else if(errorDetected == false)
        {
            Debug.Log("Audio manager instance not initialized");
            errorDetected = true;
        }
	}

	void OnLanding()
	{
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySoundSFX(land[Random.Range(0, land.Length)], gameObject, landVolume * Random.Range(0.8f, 1f), Random.Range(0.8f, 1.2f));
        }
        else
        {
            Debug.Log("Audio manager instance not initialized");
        }
    }

	void Step()
	{
		AudioManager.Instance.PlaySoundSFX(step[Random.Range(0, step.Length)], gameObject, stepVolume*Random.Range(0.4f, 0.5f), Random.Range(0.8f, 1.2f));
	}

	void StartJetpack()
	{
		jetpackSource = AudioManager.Instance.PlaySoundSFX(jetpack, gameObject, 1f, 1f, 1f, true);
		jetpackSource.time = Random.Range(0f, jetpackSource.clip.length);
	}

	void StopJetpack()
	{
		AudioManager.Instance.StopSound(jetpackSource, 0.25f);
	}


}
