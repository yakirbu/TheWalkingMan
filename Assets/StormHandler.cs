using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class StormHandler : MonoBehaviour
{
    [SerializeField]
    private GameObject stormContainer;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;
        
        foreach (Transform child in stormContainer.transform)
        {
            child.gameObject.SetActive(true);
            child.GetComponent<SpriteRenderer>().DOFade(1f, 0.3f);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        foreach (Transform child in stormContainer.transform)
        {
            child.GetComponent<SpriteRenderer>().DOFade(0f, 0.3f).OnComplete(() => child.gameObject.SetActive(false));
        }
    }
}
