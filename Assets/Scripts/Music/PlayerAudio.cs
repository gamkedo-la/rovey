﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
	public AudioClip[] jump;
	public AudioClip[] land;
	public AudioClip[] step;
	public AudioClip jetpack;

	private AudioSource jetpackSource;
	private Animator animator;

	 void Start()
	{
		animator = GetComponent<Animator>();
	}

	void StartJump()
	{
		AudioManager.Instance.PlaySoundSFX(jump[Random.Range(0, jump.Length)], gameObject, Random.Range(0.8f, 1f), Random.Range(0.8f, 1.2f));
	}

	void OnLanding()
	{
		AudioManager.Instance.PlaySoundSFX(land[Random.Range(0, land.Length)], gameObject, Random.Range(0.8f, 1f), Random.Range(0.8f, 1.2f));
	}

	void Step()
	{
		AudioManager.Instance.PlaySoundSFX(step[Random.Range(0, step.Length)], gameObject, Random.Range(0.4f, 0.5f), Random.Range(0.8f, 1.2f));
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