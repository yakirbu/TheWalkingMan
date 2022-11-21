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

    private bool IsLookingAtMemory(MemoryType memoryType)
    {
        var isLookingRight = Math.Abs(transform.localRotation.y - 0) < 0.1f;
        var memoryPosition = _memoriesManager.GetMemoryTriggerPosition(memoryType);
        return ((memoryPosition.x >= transform.position.x) && isLookingRight) ||
               ((memoryPosition.x < transform.position.x) && !isLookingRight);
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
    void OnTriggerEnter2D(Collider2D other) 
    {
        switch (other.gameObject.tag)
        {
            case "SceneOneTrigger":
                _memoryType = MemoryType.SceneOne;
                break;
            case "SceneTwoTrigger":
                _memoryType = MemoryType.SceneTwo;
                break;
            case "SceneThreeTrigger":
                _memoryType = MemoryType.SceneThree;
                break;
            case "SceneFourTrigger":
                _memoryType = MemoryType.SceneFour;
                break;
            case "SceneFiveTrigger":
                _memoryType = MemoryType.SceneFive;
                break;
            case "SceneSixTrigger":
                _memoryType = MemoryType.SceneSix;
                break;
            default:
                _memoryType = MemoryType.None;
                Debug.LogWarning("Unrecognized memory!");
                break;
        }
        Debug.Log(other.gameObject.tag);
        _isNearMemory = true;
    }
 
    void OnTriggerExit2D(Collider2D other) 
    {
        _isNearMemory = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && _memoryType != MemoryType.None)
        {
            if (_isNearMemory && IsLookingAtMemory(_memoryType))
            {
                StartCoroutine(_memoriesManager.StartMemory(_memoryType));
            }
        }
    }
}