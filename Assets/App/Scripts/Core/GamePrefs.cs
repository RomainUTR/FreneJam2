using UnityEngine;

public static class GamePrefs
{
    private const string MusicVolumeKey = "VolumMusicParam";
    private const string SFXVolumeKey = "VolumSFXParam";

    private static void SetBool(string key, bool value)
    {
        PlayerPrefs.SetInt(key, value ? 1 : 0);
    }

    private static bool GetBool(string key, bool defaultValue)
    {
        return PlayerPrefs.GetInt(key, defaultValue ? 1 : 0) == 1;
    }

    // -- Gestion du volume --

    public static float GetMusicVolume()
    {
        // Retourne la valeur stock�e, avec 1.0f comme valeur par d�faut
        return PlayerPrefs.GetFloat(MusicVolumeKey, 1.0f);
    }

    public static float GetSFXVolume()
    {
        // Retourne la valeur stock�e, avec 1.0f comme valeur par d�faut
        return PlayerPrefs.GetFloat(SFXVolumeKey, 1.0f);
    }

    public static void SetMusicVolume(float volume)
    {
        PlayerPrefs.SetFloat(MusicVolumeKey, Mathf.Clamp01(volume));
        PlayerPrefs.Save();
    }

    public static void SetSFXVolume(float volume)
    {
        PlayerPrefs.SetFloat(SFXVolumeKey, Mathf.Clamp01(volume));
        PlayerPrefs.Save();
    }
}