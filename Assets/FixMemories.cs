using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixMemories : MonoBehaviour
{
    public GameObject NoPostCamera;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Fix());
    }

    IEnumerator Fix()
    {
        yield return new WaitForSeconds(.5f);
        NoPostCamera.SetActive(false);
        yield return new WaitForSeconds(.5f);
        NoPostCamera.SetActive(true);
    }
}
