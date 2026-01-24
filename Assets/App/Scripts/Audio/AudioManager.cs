using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using DG.Tweening;
using System.Collections;
using RomainUTR.SLToolbox.Runtime;

public class AudioManager : MonoBehaviour
{
    [BoxGroup("Settings")]
    [TitleGroup("Settings/Audio")]
    [SerializeField] private int startingAudioObjectsCount;
    [TitleGroup("Settings/Audio")]
    [SerializeField, Range(0f,1f)] float transitionFadeOutDelay;
    [TitleGroup("Settings/Audio")]
    [SerializeField, Range(0f,1f)] float transitionFadeInDelay;

    [BoxGroup("References")]
    [TitleGroup("References/AudioMixer"), Required]
    [SerializeField] private AudioMixerGroup musicMixerGroup;
    [TitleGroup("References/AudioMixer"), Required]
    [SerializeField] private AudioMixerGroup soundMixerGroup;

    private Transform playlistParent = null;
    private Transform soundParent = null;
    private readonly Queue<AudioSource> soundsQueue = new();
    private List<AudioSource> audios = new();
    List<AudioSource> playlistAudios = new();
    int initialMusicCount;

    public static AudioManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        SetupAudioParent();
        for (int i = 0; i < startingAudioObjectsCount; i++)
        {
            soundsQueue.Enqueue(CreateAudioSource(soundParent));
        }
    }

    void SetupAudioParent()
    {
        playlistParent = new GameObject("PLAYLIST").transform;
        playlistParent.parent = transform;

        soundParent = new GameObject("SOUNDS").transform;
        soundParent.parent = transform;
    }

    void ClearAllAudio()
    {
        foreach(AudioSource audio in audios)
        {
            audio.Stop();
        }
    }

    public void PlayClipAt(SoundData sound, Vector3 position)
    {
        AudioSource audioSource;

        if (soundsQueue.Count <= 0)
        {
            audioSource = CreateAudioSource(soundParent);
        } else
        {
            audioSource = soundsQueue.Dequeue();
        }

        audioSource.transform.position = position;
        audioSource.clip = sound.clips.GetRandom();
        audioSource.volume = Mathf.Clamp(sound.volumeMultiplier, 0f, 1f);
        audioSource.spatialBlend = sound.spatialBlend;

        audioSource.Play();
        StartCoroutine(AddAudioSourceToQueue(audioSource));
    }

    private IEnumerator AddAudioSourceToQueue(AudioSource current)
    {
        float cooldown = current.clip.length;
        float timer = 0f;

        while (timer < cooldown)
        {
            yield return null;

            timer += Time.deltaTime;

            if (!current.isPlaying)
            {
                current.UnPause();
            }
        }

        soundsQueue.Enqueue(current);
    }

    private AudioSource CreateAudioSource(Transform parent)
    {
        AudioSource audioSource = new GameObject("AudioSource").AddComponent<AudioSource>();
        audioSource.transform.SetParent(parent);
        audioSource.outputAudioMixerGroup = soundMixerGroup;
        audios.Add(audioSource);
        return audioSource;
    }

    AudioSource CreatePlaylistAudioSource()
    {
        AudioSource audioSource = new GameObject("Playlist").AddComponent<AudioSource>();
        audioSource.transform.SetParent(playlistParent);
        audioSource.outputAudioMixerGroup = musicMixerGroup;
        return audioSource;
    }

    public void ChangeAmbianceMusic(MusicTrack[] playlists)
    {
        StartCoroutine(ChangeAmbianceMusicDelay(playlists));
    }

    IEnumerator ChangeAmbianceMusicDelay(MusicTrack[] musicDatas)
    {
        List<AudioSource> sourcesToDestroy = new List<AudioSource>();
        for (int i = initialMusicCount; i < playlistAudios.Count; i++)
        {
            if (playlistAudios[i] != null)
            {
                sourcesToDestroy.Add(playlistAudios[i]);
            }
        }

        if (playlistAudios.Count > initialMusicCount)
        {
            playlistAudios.RemoveRange(initialMusicCount, playlistAudios.Count - initialMusicCount);
        }

        foreach (AudioSource source in sourcesToDestroy)
        {
            source.DOFade(0, transitionFadeOutDelay)
                .SetLink(source.gameObject)
                .OnComplete(() =>
                {
                    if (source != null) Destroy(source.gameObject);
                });
        }

        yield return new WaitForSeconds(transitionFadeOutDelay);

        foreach (MusicTrack track in musicDatas)
        {
            if (track.music == null) continue;

            AudioSource newSource = CreatePlaylistAudioSource();
            newSource.clip = track.music.clip;
            newSource.loop = track.music.isLooping;
            newSource.volume = 0f;

            newSource.Play();

            float finalVolume = (track.music.volumMultiplier > 0 ? track.music.volumMultiplier : 1f) * track.localVolume;

            newSource.DOFade(finalVolume, transitionFadeInDelay)
                .SetLink(newSource.gameObject);

            playlistAudios.Add(newSource);
        }
    }
}
