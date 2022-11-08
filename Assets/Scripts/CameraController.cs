using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CameraController : MonoBehaviour
{
    [SerializeField] private GameObject character;

    private void FixedUpdate()
    {
        transform.position = new Vector3(character.transform.position.x, transform.position.y, transform.position.z);
    }
}
