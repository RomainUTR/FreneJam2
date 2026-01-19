using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSoundData", menuName = "Audio/Sound Data")]
public class SoundData : ScriptableObject
{
    [BoxGroup("Settings")]
    [Range(0f, 1f)] public float volumeMultiplier = 1f;
    [BoxGroup("Settings")]
    [Range(0f, 2f)] public float pitch = 1f;


    [BoxGroup("Settings")]
    [Range(0f, 1f), Tooltip("0 = 2D (UI), 1 = 3D (World)")]
    public float spatialBlend = 1f;

    [BoxGroup("Clips")]
    public AudioClip[] clips;
}
