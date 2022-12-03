using System;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerController : MonoBehaviour
{
    private static readonly int IsWalkingParam = Animator.StringToHash("isWalking");
    private static readonly int SceneOneMemory = Animator.StringToHash("MemorySceneOne");
    private static readonly int MemoryHeadAnimation = Animator.StringToHash("memoryHeadAnimation");

    public float moveSpeed = 2f;

    private float _horizontalMovement;
    private Rigidbody2D _rigidbody;
    private Animator _animator;
    private MemoriesManager _memoriesManager;

    [SerializeField]
    private GameObject arrowsGuide;
    
    private MemoryType _memoryType;

    private void Awake()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

    public void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _memoriesManager = FindObjectOfType<MemoriesManager>();
        ResetPlayerPosition();
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
            
            if(arrowsGuide.activeSelf)
                arrowsGuide.SetActive(false);
            
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
        
        _isNearMemory = true;
    }

    private bool _hideMemoryGuideButton;

    void OnTriggerExit2D(Collider2D other) 
    {
        _isNearMemory = false;
        _hideMemoryGuideButton = false;
        _memoriesManager.HideMemoryButtonGuide();
    }

    private void Update()
    {
        if (_isNearMemory)
        {
            if(!_hideMemoryGuideButton)
                _memoriesManager.ShowMemoryButtonGuide(transform.position);
            
            if (!Input.GetKeyDown(KeyCode.Space) || _memoryType == MemoryType.None) 
                return;
            
            _memoriesManager.HideMemoryButtonGuide();
            _hideMemoryGuideButton = true;
            
            if (IsLookingAtMemory(_memoryType))
            {
                //_animator.SetTrigger(MemoryHeadAnimation);
                StartCoroutine(_memoriesManager.StartMemory(_memoryType));
            }
        }
        
        if (Input.GetKeyDown(KeyCode.F))
        {
            ResetPlayerPosition();
        }
    }

    private void ResetPlayerPosition()
    {
        transform.position = new Vector3(-2.59f, -5.3f, 0);
    }
}