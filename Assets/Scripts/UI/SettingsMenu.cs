using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Slider audioSlider;
    public TMP_Dropdown resolutionDropdown;
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

        LoadFromPlayerPrefs();

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
        var adjustedVolume = Mathf.Clamp(baseVolume + volumeAdjust, -80f, 0f); // ensure bad math isn't exploding eardrums.
        audioMixer.SetFloat("volume", adjustedVolume);
        PlayerPrefs.SetFloat("volume", volume);
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        PlayerPrefs.SetInt("resolution.width", resolution.width);
        PlayerPrefs.SetInt("resolution.height", resolution.height);
        PlayerPrefs.SetInt("resolution.refreshRate", resolution.refreshRate);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        PlayerPrefs.SetInt("fullscreen", isFullscreen ? 1 : 0);
    }

    private void LoadFromPlayerPrefs()
    {
        // Load player volume settings.
        var volumeSettings = PlayerPrefs.GetFloat("volume", 1.0f);
        audioSlider.value = volumeSettings;

        // Load player fullscreen settings.
        var isFullscreen = PlayerPrefs.GetInt("fullscreen", Screen.fullScreen?1:0) == 1;
        fullscreenToggle.isOn = isFullscreen;
        
        // Load player preferred resolution settings.
        var resolution = new Resolution();
        resolution.width = PlayerPrefs.GetInt("resolution.width", Screen.width);
        resolution.height = PlayerPrefs.GetInt("resolution.height", Screen.height);
        resolution.refreshRate = PlayerPrefs.GetInt("resolution.refreshRate", Screen.currentResolution.refreshRate);
        Screen.SetResolution(resolution.width, resolution.height, isFullscreen, resolution.refreshRate);
        Screen.SetResolution(resolution.width, resolution.height, isFullscreen);
    }
}
