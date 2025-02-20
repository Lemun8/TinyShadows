using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class Settings : MonoBehaviour
{
    public Slider bgmSlider;
    public Slider sfxSlider;
    public AudioMixer audioMixer;

    private const string BGMVolumeKey = "BGMVolume";
    private const string SFXVolumeKey = "SFXVolume";

    void Start()
    {
        // Load saved volumes and set sliders accordingly
        bgmSlider.value = PlayerPrefs.GetFloat(BGMVolumeKey, 0.75f);
        sfxSlider.value = PlayerPrefs.GetFloat(SFXVolumeKey, 0.75f);

        // Set the initial volume levels in the AudioMixer
        SetBGMVolume(bgmSlider.value);
        SetSFXVolume(sfxSlider.value);

        // Add listeners for slider value changes
        bgmSlider.onValueChanged.AddListener(SetBGMVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);
    }

    public void SetBGMVolume(float volume)
    {
        Debug.Log("Setting BGM Volume: " + volume);
        PlayerPrefs.SetFloat(BGMVolumeKey, volume);
        PlayerPrefs.Save();
        float dbVolume = Mathf.Log10(Mathf.Clamp(volume, 0.0001f, 1)) * 20;
        Debug.Log("BGM dB Volume: " + dbVolume);
        audioMixer.SetFloat(BGMVolumeKey, dbVolume);
    }

    public void SetSFXVolume(float volume)
    {
        Debug.Log("Setting SFX Volume: " + volume);
        PlayerPrefs.SetFloat(SFXVolumeKey, volume);
        PlayerPrefs.Save();
        float dbVolume = Mathf.Log10(Mathf.Clamp(volume, 0.0001f, 1)) * 20;
        Debug.Log("SFX dB Volume: " + dbVolume);
        audioMixer.SetFloat(SFXVolumeKey, dbVolume);
    }
}
