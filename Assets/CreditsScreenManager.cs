using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsScreenManager : MonoBehaviour
{
    private bool _pressed;

    private void Start()
    {
        _pressed = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!Input.GetKeyDown(KeyCode.Space) || _pressed) 
            return;
        
        _pressed = true;
        var fader = CameraFader.NewFadeOut(0.5f);
        var asyncOperation = SceneManager.LoadSceneAsync("Scenes/opening_scene", LoadSceneMode.Single);
        asyncOperation.allowSceneActivation = false;
        fader.endCall = () =>
        {
            asyncOperation.allowSceneActivation = true;
            CameraFader.NewFadeIn(0.5f);
        };
    }
}
