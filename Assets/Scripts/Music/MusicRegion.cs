using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicRegion : MonoBehaviour {
	
	public bool startMusicOnTriggerEnter = false;
	public bool stopMusicOnTriggerExit = false;
	public float fadeTime = 2f;

	public bool hardOutOnTriggerEnter = false;
	public bool hardOutOnTriggerExit = false;

	public MusicPool pool;

	public string tagFilter;

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == tagFilter) {
			MusicManager.Instance.PushMusicPool(pool);
			if (startMusicOnTriggerEnter) MusicManager.Instance.StartMusic();
			if (hardOutOnTriggerEnter) MusicManager.Instance.TransitionMusicNow();
		}
	}

	void OnTriggerExit(Collider other) {
		if (other.gameObject.tag == tagFilter)
			MusicManager.Instance.PopMusicPool();
		if (stopMusicOnTriggerExit) MusicManager.Instance.FadeOutMusic(fadeTime);
		if (hardOutOnTriggerExit) MusicManager.Instance.TransitionMusicNow();
	}
}
