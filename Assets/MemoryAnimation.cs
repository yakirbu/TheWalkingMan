
using UnityEngine;

public class MemoryAnimation : MonoBehaviour
{
    private MemoriesManager _memoriesManager;
    private void Start()
    {
        _memoriesManager = FindObjectOfType<MemoriesManager>();
    }
    
    public void MemoryEnd()
    {
        _memoriesManager.OnMemoryEnd(gameObject);
    }
}
