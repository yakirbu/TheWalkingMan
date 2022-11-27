using UnityEngine;
using System.Collections;

// For usage apply the script directly to the element you wish to apply parallaxing
// Based on Brackeys 2D parallaxing script http://brackeys.com/
public class Parallax : MonoBehaviour {
    private float length, startPos;
    public GameObject cam;
    public float parallaxEffect;
    public SpriteRenderer floor;
    // private float _boundMargin = 10f;
    
    void Start () {
        startPos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }
    
    void Update ()
    {
        float dist = (cam.transform.position.x * parallaxEffect);
        float newX = dist + startPos + length;
        print($"new_x = {newX} ({gameObject.name})\n length = {length} ({gameObject.name})\n max_x = {floor.bounds.max.x}");
        if (newX < floor.bounds.max.x)
        {
            transform.position = new Vector3(newX, transform.position.y, transform.position.z);
        }
    }
}