using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using Debug = System.Diagnostics.Debug;

public enum MemoryType
{
    None,
    SceneOne,
    SceneTwo,
    SceneThree,
    SceneFour,
    SceneFive,
    SceneSix,
}

[Serializable]
public class Memory
{
    public MemoryType memoryType;
    public Animator memoryAnimator;
    public GameObject memoryTrigger;
}

public class MemoriesManager : MonoBehaviour
{
    private static readonly int PlayAnimationParam = Animator.StringToHash("PlayAnimation");
    
    [SerializeField]
    private List<Memory> memoriesList = new();

    private readonly Dictionary<MemoryType, Memory> _memories = new();

    [SerializeField]
    private SpriteRenderer memoryButtonGuide;
    
    internal bool IsMemoryPlaying;
    
    private PostProcessVolume _postProcess;
    private ColorGrading _colorGrading;

    private void Awake()
    {
        foreach (var memory in memoriesList)
        {
            _memories[memory.memoryType] = memory;
        }

        memoriesList = null;
        
        _postProcess = FindObjectOfType<PostProcessVolume>();
        _postProcess.profile.TryGetSettings(out _colorGrading);
    }

    public void ShowMemoryButtonGuide(Vector2 memoryButtonPosition)
    {
        var parent = memoryButtonGuide.transform.parent;
        parent.position = new Vector2(memoryButtonPosition.x + memoryButtonGuide.size.x, parent.position.y);
        parent.gameObject.SetActive(true);
        memoryButtonGuide.DOFade(1f, 0.1f);
    }

    public void HideMemoryButtonGuide()
    {
        memoryButtonGuide.DOFade(0f, 0.1f).OnComplete((() => memoryButtonGuide.transform.parent.gameObject.SetActive(false)));
    }

    public void OnMemoryEnd(GameObject memoryGameObject)
    {
        memoryGameObject.GetComponent<SpriteRenderer>().DOFade(0f, 0.5f).OnComplete(() =>
        {
            memoryGameObject.SetActive(false);
            memoryGameObject.GetComponent<Animator>().SetBool(PlayAnimationParam, false);
            IsMemoryPlaying = false;
        });
    }

    public Vector2 GetMemoryTriggerPosition(MemoryType memoryType)
    {
        if (_memories.ContainsKey(memoryType))
            return _memories[memoryType].memoryTrigger.transform.position;
        
        return Vector2.zero;
    }
    
    public IEnumerator StartMemory(MemoryType memoryType)
    {
        if (IsMemoryPlaying)
            yield break;
        
        IsMemoryPlaying = true;

        var animator = _memories[memoryType].memoryAnimator;
        
        animator.gameObject.SetActive(true);

        animator.SetBool(PlayAnimationParam, true);
        animator.gameObject.GetComponent<SpriteRenderer>().DOFade(1f, .5f);

        float animTime = GetCurrentAnimTime(animator);

        // if (saturationTween.IsPlaying())
        //     yield return new WaitWhile(() => saturationTween.IsPlaying());
        
        DOTween.To(() => _colorGrading.saturation, x => _colorGrading.saturation.value = x, 0f, animTime * 3/4).OnComplete(() =>
        {
            DOTween.To(() => _colorGrading.saturation, x => _colorGrading.saturation.value = x,
                -100f, animTime * 1.2f);
        });
        
    }

    private float GetCurrentAnimTime(Animator animator)
    {
        return animator.runtimeAnimatorController.animationClips[0].length;
    }
    
}
