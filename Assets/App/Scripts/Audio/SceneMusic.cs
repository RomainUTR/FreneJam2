using UnityEngine;
using Sirenix.OdinInspector;

public class SceneMusic : MonoBehaviour
{
    [Title("Playlist & Mixage")]
    [ListDrawerSettings(ShowIndexLabels = false)]
    [SerializeField] private MusicTrack[] musicLayers;

    [Title("Settings")]
    [SerializeField] private bool playOnStart = true;

    private void Start()
    {
        if (playOnStart && AudioManager.Instance != null)
        {
            AudioManager.Instance.ChangeAmbianceMusic(musicLayers);
        }
    }

    public void TriggerMusic()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.ChangeAmbianceMusic(musicLayers);
        }
    }
}
