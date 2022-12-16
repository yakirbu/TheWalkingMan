using UnityEngine;
using System.Collections;

// For usage apply the script directly to the element you wish to apply parallaxing
// Based on Brackeys 2D parallaxing script http://brackeys.com/
public class Parallax : MonoBehaviour
{
    private float length, startpos;
    public GameObject cam;
    public float parallaxEffect;
    private PlayerController _playerController;
    private float camPosOffset;
    

    void Start()
    {
        startpos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
        _playerController = FindObjectOfType<PlayerController>();
        camPosOffset = cam.transform.position.x;
    }

    private bool first = true;
    void Update()
    {
        if(Mathf.Abs(_playerController.transform.position.x - transform.position.x) > 15)
        {
            return;
        }
        if (first)
        {
            camPosOffset = cam.transform.position.x;
            first = false;
        }
        float dist = ((cam.transform.position.x - camPosOffset) * parallaxEffect);
        transform.position = new Vector3(startpos + dist, transform.position.y, transform.position.z);
    }
}