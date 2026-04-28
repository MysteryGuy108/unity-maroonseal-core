using System.Collections;
using TMPro;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

namespace MaroonSeal.Routines
{
    public class MaroonSealSplashscreenPlayer : RoutinePlayer
    {
        [Header("Video")]
        [SerializeField] private VideoPlayer videoPlayer;
        [SerializeField] private RawImage videoRender;
        [Space]
        [SerializeField] private float fadeInTime = 1.0f;
        [SerializeField] private float fadeOutTime = 1.0f;

        [Header("Cursor")]
        [SerializeField] private bool hideCursorAtStart;
        [SerializeField] private bool showCursorAtEnd;

        [Header("Scene Management")]
        [SerializeField] private int nextSceneBuildIndex;

        #region MonoBehaviour
        private void Start()
        {
            videoPlayer.targetTexture.Release();

            videoRender.texture = videoPlayer.targetTexture;
            videoRender.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);

            videoPlayer.Prepare();
            videoPlayer.prepareCompleted += cntx => Play(PlaySplashscreen());
        }
        #endregion

        private IEnumerator PlaySplashscreen()
        {
            Cursor.visible = !hideCursorAtStart;
            yield return null;

            AsyncOperation loadOperation = SceneManager.LoadSceneAsync(nextSceneBuildIndex);
            loadOperation.allowSceneActivation = false;

            yield return new WaitForSeconds(1.0f);
            videoPlayer.Play();

            yield return new PlayAndWaitForGraphicFade(videoRender, fadeInTime, 1.0f);
            yield return new WaitForSeconds(((float)videoPlayer.length / videoPlayer.playbackSpeed) - fadeInTime);

            yield return new WaitForSeconds(1.0f);
            yield return new PlayAndWaitForGraphicFade(videoRender, fadeOutTime, 0.0f);
            yield return new WaitForSeconds(1.0f);

            Cursor.visible = showCursorAtEnd;
            loadOperation.allowSceneActivation = true;
        }
    }
}

