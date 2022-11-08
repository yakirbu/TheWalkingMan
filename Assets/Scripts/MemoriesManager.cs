using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public enum MemoryType
{
    None,
    SceneOne,
}

[Serializable]
public class Memory
{
    public MemoryType memoryType;
    public Animator memoryAnimator;
}

public class MemoriesManager : MonoBehaviour
{
    private static readonly int PlayAnimationParam = Animator.StringToHash("PlayAnimation");
    
    [SerializeField]
    private List<Memory> memoriesList = new();

    private readonly Dictionary<MemoryType, Animator> _memories = new Dictionary<MemoryType, Animator>();

    internal bool IsMemoryPlaying;
    
    private PostProcessVolume _postProcess;
    private ColorGrading _colorGrading;

    
    private void Awake()
    {
        foreach (var memory in memoriesList)
        {
            _memories[memory.memoryType] = memory.memoryAnimator;
        }

        memoriesList = null;
        
        _postProcess = FindObjectOfType<PostProcessVolume>();
        _postProcess.profile.TryGetSettings(out _colorGrading);
    }

    public void OnMemoryEnd(GameObject memoryGameObject)
    {
        IsMemoryPlaying = false;
        memoryGameObject.GetComponent<SpriteRenderer>().DOFade(0f, 0.5f).OnComplete(() =>
        {
            memoryGameObject.SetActive(false);
            memoryGameObject.GetComponent<Animator>().SetBool(PlayAnimationParam, false);
        });

    }

    public IEnumerator StartMemory(MemoryType memoryType)
    {
        if (IsMemoryPlaying)
            yield break;
        
        IsMemoryPlaying = true;

        var animator = _memories[memoryType];
        
        animator.gameObject.SetActive(true);

        animator.SetBool(PlayAnimationParam, true);
        animator.gameObject.GetComponent<SpriteRenderer>().DOFade(1f, .5f);
        
        //.Interp(_colorGrading.saturation.value, 100, 1f);
        DOTween.To(() => _colorGrading.saturation, x => _colorGrading.saturation.value = x, -100f, .5f).OnComplete(() =>
        {
            DOTween.To(() => _colorGrading.saturation, x => _colorGrading.saturation.value = x,
                0, GetCurrentAnimTime(animator));
        });
    }

    private float GetCurrentAnimTime(Animator animator)
    {
        var currentClipInfo = animator.GetCurrentAnimatorClipInfo(0);
        var currentClipLength = currentClipInfo[0].clip.length;
        return currentClipLength;
    }
    
}
