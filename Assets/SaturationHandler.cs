using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaturationHandler : MonoBehaviour
{
    [SerializeField]
    private MemoriesManager memoriesManager;

    [SerializeField] private AudioSource hornSound;

    public void Awake()
    {
        DontDestroyOnLoad(this);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        memoriesManager.Saturate(0f, 1f);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.transform.position.x > GetComponent<Collider2D>().bounds.center.x)
        {
            StartCoroutine(MoveToMainMenu());
        }
        else
        {
            memoriesManager.Saturate(-100f, 1f);
        }
    }

    IEnumerator MoveToMainMenu()
    {
        hornSound.Play();
        var fader = CameraFader.NewFadeOut(2f, waitUntilDestroy: 6f);

        yield return new WaitForSeconds(7f);
 
        var asyncOperation = SceneManager.LoadSceneAsync("Scenes/opening_scene", LoadSceneMode.Single);
        asyncOperation.completed += operation =>
        {
            CameraFader.NewFadeIn(1f).endCall = () =>
            {
                Destroy(this);
            };
        };
    }
}
