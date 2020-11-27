using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject pauseMenu;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Pause"))
        {
            TogglePauseMenu();
        }
    }

    public void TogglePauseMenu()
    {
        if (pauseMenu)
        {
            pauseMenu.SetActive(!pauseMenu.activeInHierarchy);
        }
        else
        {
            Debug.LogError("No pause menu assigned in HUD.UIManager");
        }
    }
}
