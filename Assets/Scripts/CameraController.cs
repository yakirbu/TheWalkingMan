using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CameraController : MonoBehaviour
{
    [SerializeField] private GameObject character;
    [SerializeField] private SpriteRenderer leftMostBackground;
    [SerializeField] private SpriteRenderer rightMostBackground;

    private Vector3 _newCameraPos;
    private float _leftXBound;
    private float _rightXBound;
    private Camera _camera;
    private float _centerOffset;
    
    private void Awake()
    {
        _leftXBound = leftMostBackground.bounds.min.x;
        _rightXBound = rightMostBackground.bounds.max.x;

        _camera = GetComponent<Camera>();
        // Calculate half of the horizontal size the camera can see 
        _centerOffset = _camera.orthographicSize * Screen.width / Screen.height;
        _newCameraPos = new Vector3(character.transform.position.x, transform.position.y, transform.position.z);
    }

    private void FixedUpdate()
    {
        _newCameraPos.x = Mathf.Clamp(character.transform.position.x, 
            _leftXBound + _centerOffset, 
            _rightXBound - _centerOffset);

        transform.position = _newCameraPos;
    }
}
