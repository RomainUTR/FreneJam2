using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using Sirenix.OdinInspector;
using DG.Tweening;

namespace RomainUTR.Template.Core
{
    public class GameBootstrapper : MonoBehaviour
    {
        [Title("Configuration")]
        [SceneObjectsOnly]
        public string nextSceneName = "MainMenu";

        [Range(0f, 5f)]
        public float minLoadingTime = 2f;

        [Title("Transition")]
        [Required]
        [Tooltip("Drag here the CanvasGroup for fade")]
        public CanvasGroup fadeOverlay;

        [Title("Logo Transform")]
        [Required]
        public Transform logoTransform;

        [Range(0.1f, 2f)]
        public float fadeDuration = 1f;

        private IEnumerator Start()
        {
            fadeOverlay.alpha = 1f;
            DOTween.Init();

            logoTransform.DOScale(1.1f, 0.75f)
                .SetEase(Ease.InOutSine)
                .SetLoops(-1, LoopType.Yoyo);

            yield return fadeOverlay.DOFade(0f, fadeDuration).WaitForCompletion();

            AsyncOperation operation = SceneManager.LoadSceneAsync(nextSceneName);
            operation.allowSceneActivation = false; 

            float timer = 0f;

            while (operation.progress < 0.9f || timer < minLoadingTime)
            {
                timer += Time.deltaTime;
                yield return null;
            }

            yield return fadeOverlay.DOFade(1f, fadeDuration).WaitForCompletion();
            logoTransform.DOKill(true);

            operation.allowSceneActivation = true;
        }
    }
}