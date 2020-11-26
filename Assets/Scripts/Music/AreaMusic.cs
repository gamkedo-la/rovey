using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaMusic : MonoBehaviour
{
	public bool hardOutOnSceneEnter = false;
	public bool hardOutOnSceneExit = false;

	public MusicPool pool;
	public MusicTrack[] tracks = new MusicTrack[0];

	void Start()
	{
		MusicManager.Instance.PushMusicPool(pool);
		if (hardOutOnSceneEnter) MusicManager.Instance.TransitionMusicNow();
	}

	void OnDestroy()
	{
		MusicManager.Instance.PopMusicPool();
		if (hardOutOnSceneExit) MusicManager.Instance.TransitionMusicNow();
	}
}
