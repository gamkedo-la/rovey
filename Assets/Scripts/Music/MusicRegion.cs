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
	public MusicTrack[] tracks = new MusicTrack[0];

	public LayerMask layers;

	private void Start()
	{
		if (pool == null && tracks.Length == 0)
		{
			gameObject.SetActive(false);
		}
		else if (pool == null)
		{
			pool = new MusicPool();
			pool.musicStems = new MusicTrack[tracks.Length];
			pool.musicStems = tracks;
		}
	}

	void OnTriggerEnter(Collider other) {
		if (0 != (layers.value & 1 << other.gameObject.layer)) {
			MusicManager.Instance.PushMusicPool(pool);
			if (startMusicOnTriggerEnter) MusicManager.Instance.StartMusic();
			if (hardOutOnTriggerEnter) MusicManager.Instance.TransitionMusicNow();
		}
	}

	void OnTriggerExit(Collider other) {
		if (0 != (layers.value & 1 << other.gameObject.layer))
			MusicManager.Instance.PopMusicPool();
		if (stopMusicOnTriggerExit) MusicManager.Instance.FadeOutMusic(fadeTime);
		if (hardOutOnTriggerExit) MusicManager.Instance.TransitionMusicNow();
	}
}
