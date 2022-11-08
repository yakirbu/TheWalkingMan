using System;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerController : MonoBehaviour
{
    private static readonly int IsWalkingParam = Animator.StringToHash("isWalking");
    private static readonly int SceneOneMemory = Animator.StringToHash("MemorySceneOne");
    
    public float moveSpeed = 2f;

    private float _horizontalMovement;
    private Rigidbody2D _rigidbody;
    private Animator _animator;
    private MemoriesManager _memoriesManager;
    
    private MemoryType _memoryType;
    
    public void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _memoriesManager = FindObjectOfType<MemoriesManager>();
    }

    private bool CanMove()
    {
        return _horizontalMovement != 0 && !_memoriesManager.IsMemoryPlaying;
    }
    
    public void FixedUpdate()
    {
        if(CanMove())
        {
            transform.localRotation = Quaternion.Euler(0, _horizontalMovement < 0 ? 180 : 0, 0);
            MovePlayer(new Vector2(_horizontalMovement, 0));

            SetWalkingAnimation(true);
        } 
        else
        {
            SetWalkingAnimation(false);
        }
    }

    private void SetWalkingAnimation(bool isWalking)
    {
        _animator.SetBool(IsWalkingParam, isWalking);
    }
    
    private void MovePlayer(Vector2 direction)
    {
        Vector2 moveVector = direction * moveSpeed * Time.fixedDeltaTime;
        _rigidbody.MovePosition(_rigidbody.position + moveVector);
    }
    
    public void OnMove(InputValue value)
    {
        _horizontalMovement = value.Get<Vector2>().x;
    }


    bool _isNearMemory = false;
    void OnTriggerEnter2D(Collider2D other) {
        
        switch (other.gameObject.tag)
        {
            case "SceneOneTrigger":
                _memoryType = MemoryType.SceneOne;
                break;
            default:
                _memoryType = MemoryType.None;
                break;
        }
        Debug.Log(other.gameObject.tag);
        _isNearMemory = true;

    }
 
    void OnTriggerExit2D(Collider2D other) {
        _isNearMemory = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && _memoryType != MemoryType.None)
        {
            if(_isNearMemory)
                StartCoroutine(_memoriesManager.StartMemory(_memoryType));
        }
    }
    

}