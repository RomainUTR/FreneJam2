using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PlayerFeedback : MonoBehaviour
{
    public AudioMixer mainMixer;
    public TrailRenderer shipTrail;
    public Volume globalVolume;

    public Color driftColor = Color.white;
    public Color thrustColor;
    public float transitionSpeed = 5f;

    private ChromaticAberration chromaticAberration;
    private bool isThrusting;

    private void Start()
    {
        if (globalVolume.profile.TryGet(out ChromaticAberration ca))
        {
            chromaticAberration = ca;
        }
    }

    public void UpdateFeedback(bool thrustInput)
    {
        isThrusting = thrustInput;
        float deltaTime = Time.deltaTime * transitionSpeed;

        float targetFreq = isThrusting ? 22000f : 500f;
        float currentFreq;
        mainMixer.GetFloat("LowPass", out currentFreq);
        float newFreq = Mathf.Lerp(currentFreq, targetFreq, deltaTime);

        mainMixer.SetFloat("LowPass", newFreq);

        if (shipTrail)
        {
            Color targetCol = isThrusting ? thrustColor : driftColor;
            shipTrail.startColor = Color.Lerp(shipTrail.startColor, targetCol, deltaTime);
        }

        if (chromaticAberration != null)
        {
            float targetDistortion = isThrusting ? 0.5f : 0f;

            float currentDistortion = chromaticAberration.intensity.value;
            chromaticAberration.intensity.value = Mathf.Lerp(currentDistortion, targetDistortion, deltaTime);
        }
    }
}
