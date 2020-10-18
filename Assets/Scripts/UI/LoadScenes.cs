using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScenes : MonoBehaviour
{

    public string SceneToLoad;
    public void LoadScene(string SceneName)
    {
        Debug.Log("Load scene called");
        SceneManager.LoadScene(SceneName);
    }
}
