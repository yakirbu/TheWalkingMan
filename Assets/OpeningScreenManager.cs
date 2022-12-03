using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OpeningScreenManager : MonoBehaviour
{
    [SerializeField]
    private Button playButton;
    [SerializeField]
    private Button creditsButton;
    
    void Start()
    {
        playButton.onClick.AddListener(() =>
        {            
            var fader = CameraFader.NewFadeOut(0.5f);
            var asyncOperation = SceneManager.LoadSceneAsync("Scenes/main_scene", LoadSceneMode.Single);
            asyncOperation.allowSceneActivation = false;
            fader.endCall = () =>
            {
                asyncOperation.allowSceneActivation = true;
                CameraFader.NewFadeIn(0.5f);
            };
        });
        
        creditsButton.onClick.AddListener(() =>
        {
            var fader = CameraFader.NewFadeOut(0.5f);
            var asyncOperation = SceneManager.LoadSceneAsync("Scenes/credits_scene", LoadSceneMode.Single);
            asyncOperation.allowSceneActivation = false;
            fader.endCall = () =>
            {
                asyncOperation.allowSceneActivation = true;
                CameraFader.NewFadeIn(0.5f);
            };
        });
    }
}
