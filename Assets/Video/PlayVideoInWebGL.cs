using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
public class PlayVideoInWebGL : MonoBehaviour
{

    [SerializeField]
    private VideoPlayer videoPlayer;

    [SerializeField]
    private string videoFileName;

    private string url;
    // Start is called before the first frame update
    void Start()
    {
        if (string.IsNullOrEmpty(url))
        {
            url = System.IO.Path.Combine(Application.streamingAssetsPath, videoFileName);
        }
        videoPlayer.url = url;
    }


}
