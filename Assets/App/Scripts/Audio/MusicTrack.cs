using Sirenix.OdinInspector;
using UnityEngine;

[System.Serializable]
[InlineProperty]
public class MusicTrack 
{
    [HorizontalGroup("Row", Width = 0.6f)]
    [HideLabel, AssetsOnly]
    public MusicData music;

    [HorizontalGroup("Row")]
    [HideLabel]
    [Range(0f, 1f)]
    [GUIColor(0.7f, 1f, 0.7f)]
    [Tooltip("Volume local pour cette scène")]
    public float localVolume = 1f;
}
