using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class AudioController : MonoBehaviour
{
    [SerializeField, Required] private AudioMixer audioMixer;
    [SerializeField, Required] private Slider musicSlider, sfxSlider;

    private void OnEnable()
    {
        audioMixer.SetFloat("MasterPitch", 1f);
        audioMixer.SetFloat("LowPass", 22000f);

        float musicVolumeLin = GamePrefs.GetMusicVolume();
        SetMusicVolumeInternal(musicVolumeLin);
        musicSlider.value = musicVolumeLin;

        float sfxVolumeLin = GamePrefs.GetSFXVolume();
        SetSFXVolumeInternal(sfxVolumeLin);
        sfxSlider.value = sfxVolumeLin;

        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);
    }

    private void SetMusicVolumeInternal(float volumeLin)
    {
        float volumeInDB;

        if (volumeLin <= 0.0001f)
        {
            volumeInDB = -80f;
        }
        else
        {
            volumeInDB = Mathf.Log10(volumeLin) * 20;
        }

        audioMixer.SetFloat("VolumMusicParam", volumeInDB);
    }

    public void SetMusicVolume(float volumeLin)
    {
        SetMusicVolumeInternal(volumeLin);
        GamePrefs.SetMusicVolume(volumeLin);
    }

    private void SetSFXVolumeInternal(float volumeLin)
    {
        float volumeInDB;

        if (volumeLin <= 0.0001f)
        {
            volumeInDB = -80f;
        }
        else
        {
            volumeInDB = Mathf.Log10(volumeLin) * 20;
        }

        audioMixer.SetFloat("VolumSFXParam", volumeInDB);
    }

    public void SetSFXVolume(float volumeLin)
    {
        SetSFXVolumeInternal(volumeLin);
        GamePrefs.SetSFXVolume(volumeLin);
    }
}