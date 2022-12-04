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

    internal bool IsMemoryPlaying2;
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

    public void ShowMemoryButtonGuide()
    {
        var parent = memoryButtonGuide.transform.parent;
        parent.gameObject.SetActive(true);
        //yield return new WaitForSeconds(0.5f);
        var color = memoryButtonGuide.color;
        color.a = 0;
        memoryButtonGuide.color = color;
        memoryButtonGuide.DOFade(1f, .5f);
    }
    
    public void UpdateMemoryButtonPosition(Vector2 memoryButtonPosition)
    {
        var parent = memoryButtonGuide.transform.parent;
        parent.position = new Vector2(memoryButtonPosition.x + memoryButtonGuide.size.x, parent.position.y);
    }
    
    

    public void HideMemoryButtonGuide()
    {
        memoryButtonGuide.DOFade(0f, 0.25f).OnComplete((() =>
        {
            memoryButtonGuide.transform.parent.gameObject.SetActive(false);
        }));
    }

    public void OnMemoryEnd(GameObject memoryGameObject)
    {
        memoryGameObject.GetComponent<SpriteRenderer>().DOFade(0f, 0.5f).OnComplete(() =>
        {
            memoryGameObject.SetActive(false);
            memoryGameObject.GetComponent<Animator>().SetBool(PlayAnimationParam, false);
            IsMemoryPlaying2 = false;
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
        if (IsMemoryPlaying2)
            yield break;
        
        IsMemoryPlaying = true;
        IsMemoryPlaying2 = true;

        var animator = _memories[memoryType].memoryAnimator;
        
        animator.gameObject.SetActive(true);

        animator.SetBool(PlayAnimationParam, true);
        animator.gameObject.GetComponent<SpriteRenderer>().DOFade(1f, .5f);

        float animTime = GetCurrentAnimTime(animator);

        // if (saturationTween.IsPlaying())
        //     yield return new WaitWhile(() => saturationTween.IsPlaying());

       
        Saturate(0f, animTime * 3 / 4).OnComplete(() =>
        {
            Saturate(-100f, animTime * 1.1f).OnComplete(()=> IsMemoryPlaying = false);
        });
    }

    public TweenerCore<float, float, FloatOptions> Saturate(float endValue, float duration)
    {
        return DOTween.To(() => _colorGrading.saturation, x => _colorGrading.saturation.value = x, endValue, duration);
    }
    
    private float GetCurrentAnimTime(Animator animator)
    {
        return animator.runtimeAnimatorController.animationClips[0].length;
    }
    
}
