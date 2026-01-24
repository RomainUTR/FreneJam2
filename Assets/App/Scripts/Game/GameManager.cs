using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField, Required] private TMP_Text scoreText;
    [SerializeField, Required] private TMP_Text highScoreInGameText;
    [SerializeField, Required] private TMP_Text highScoreInMenuText;
    [SerializeField, Required] private TMP_Text finalScoreText;

    [SerializeField, Required] private GameObject gameOverPanel;

    [ReadOnly] public float scoreTimer = 0f;

    [SerializeField] private Color recordColor;

    private const string HighScore_Key = "BestScore";
    private bool isGameOver = false;

    public Volume globalVolume;
    public AudioMixer mainMixer;

    private void Start()
    {
        float bestTime = PlayerPrefs.GetFloat(HighScore_Key);
        highScoreInGameText.text = bestTime.ToString("F2") + "s";
        highScoreInGameText.color = recordColor;
    }

    private void Update()
    {
        if (isGameOver) return;
        scoreTimer += Time.deltaTime;
        if (scoreText) scoreText.text = scoreTimer.ToString("F1") + "s";
    }

    public void TriggerGameOver()
    {
        if (isGameOver) return;
        isGameOver = true;

        float bestTime = PlayerPrefs.GetFloat(HighScore_Key, 0f);
        bool isNewRecord = false;

        if (scoreTimer > bestTime)
        {
            bestTime = scoreTimer;
            isNewRecord = true;
            PlayerPrefs.SetFloat(HighScore_Key, bestTime);
            PlayerPrefs.Save();
        }

        if (finalScoreText)
        {

            finalScoreText.text = "<color=#888888>TIME: </color>" + scoreTimer.ToString("F2") + "s";

            if (isNewRecord)
            {
                finalScoreText.color = recordColor;
            }
            else
            {
                finalScoreText.color = Color.white;
            }
        }

        if (highScoreInMenuText)
        {
            if (isNewRecord)
            {
                highScoreInMenuText.text = "> NEW ORBIT DATA <";
                highScoreInMenuText.color = recordColor;
            }
            else
            {
                highScoreInMenuText.text = "<color=#888888>BEST: </color>" + bestTime.ToString("F2") + "s";
                highScoreInMenuText.color = Color.white;
            }
        }

        StartCoroutine(GameOverSequence());
    }

    IEnumerator GameOverSequence()
    {
        Time.timeScale = 0.1f;

        float currentPitch;
        mainMixer.GetFloat("MasterPitch", out currentPitch);
        mainMixer.DOSetFloat("MasterPitch", 0.4f, 1.5f).SetUpdate(true);

        mainMixer.DOSetFloat("LowPass", 100f, 1.5f).SetUpdate(true);

        if (globalVolume.profile.TryGet(out ColorAdjustments colorAdj))
        {
            DOTween.To(() => colorAdj.saturation.value, x => colorAdj.saturation.value = x, -100f, 0.2f)
                .SetUpdate(true);
        }

        if (globalVolume.profile.TryGet(out Vignette vig))
        {
            DOTween.To(() => vig.intensity.value, x => vig.intensity.value = x, 1f, 0.8f)
                .SetUpdate(true)
                .SetEase(Ease.InExpo);
        }

        if (globalVolume.profile.TryGet(out LensDistortion lens))
        {
            DOTween.To(() => lens.intensity.value, x => lens.intensity.value = x, -0.8f, 0.8f)
                .SetUpdate(true);
        }

        yield return new WaitForSecondsRealtime(1.2f);

        if (gameOverPanel)
        {
            gameOverPanel.SetActive(true);
            Time.timeScale = 1f; 
        }
    }

    public void GameQuit()
    {
        Application.Quit();
    }

    public void RetryGame()
    {
        mainMixer.SetFloat("MasterPitch", 1f);
        DOTween.KillAll();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void BackToMenu()
    {
        mainMixer.SetFloat("MasterPitch", 1f);
        mainMixer.SetFloat("LowPass", 22000f);
        DOTween.KillAll();
        SceneManager.LoadScene("Main");
    }
}
