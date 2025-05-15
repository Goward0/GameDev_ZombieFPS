using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    public GameObject optionsPanel;
    public GameObject mainMenuPanel;

    public Slider masterVolumeSlider;
    public Slider sfxVolumeSlider;
    public Slider mouseSensitivitySlider;

    void Start()
    {
        // Load saved values or defaults
        masterVolumeSlider.value = PlayerPrefs.GetFloat("MasterVolume", 1f);
        sfxVolumeSlider.value = PlayerPrefs.GetFloat("SFXVolume", 1f);
        mouseSensitivitySlider.value = PlayerPrefs.GetFloat("MouseSensitivity", 5f);
    }

    public void ApplySettings()
    {
        // Save settings
        PlayerPrefs.SetFloat("MasterVolume", masterVolumeSlider.value);
        PlayerPrefs.SetFloat("SFXVolume", sfxVolumeSlider.value);
        PlayerPrefs.SetFloat("MouseSensitivity", mouseSensitivitySlider.value);
        PlayerPrefs.Save();

        // Apply settings
        AudioListener.volume = masterVolumeSlider.value;
    }

    public void ShowOptions()
    {
        optionsPanel.SetActive(true);
        mainMenuPanel.SetActive(false);
    }

    public void BackToMainMenu()
    {
        ApplySettings();
        optionsPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }
}
