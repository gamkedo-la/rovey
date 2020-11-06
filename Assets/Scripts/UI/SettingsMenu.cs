using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Dropdown resolutionDropdown;
    public Toggle fullscreenToggle;

    private Resolution[] resolutions;
    private float baseVolume;

    private void Start()
    {
        // Get base volume from audio mixer.
        audioMixer.GetFloat("volume", out baseVolume);

        // Get available resolutions for device.
        resolutions = Screen.resolutions;

        // Set correct default value for fullscreen toggle.
        fullscreenToggle.isOn = Screen.fullScreen;

        // Populate resolution dropdown options.
        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();
        int currentResolutionIndex = 0;
        for (var i = 0; i < resolutions.Length; i++)
        {
            var resolution = resolutions[i];
            if (Screen.currentResolution.width == resolution.width &&
                Screen.currentResolution.height == resolution.height)
            {
                currentResolutionIndex = i;
            }
            options.Add($"{resolution.width} x {resolution.height}");
        }
        resolutionDropdown.AddOptions(options);

        // Select correct default value for resolution dropdown.
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    public void AdjustVolume(float volume)
    {
        var volumeAdjust = volume == 0 ? -80f : 10 * Mathf.Log10(volume);
        var adjustedVolume = Mathf.Clamp(volume + volumeAdjust, -80f, 0f); // ensure bad math isn't exploding eardrums.
        audioMixer.SetFloat("volume", adjustedVolume);
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }
}
