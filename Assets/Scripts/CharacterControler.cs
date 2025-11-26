
using UnityEngine;
using UnityEngine.InputSystem;

public class MeQuieroMorir : MonoBehaviour
{

    Animator _animator;

    CharacterController _controller;

    InputAction _moveAction;

    Camera _cam;

    InputAction _jumpAction;

    Vector2 _moveValue;

    [SerializeField] float _movementSpeed = 5;

    [SerializeField] float _jumpHight = 2;

    [SerializeField] float _gravity = -9.81f;

    [SerializeField] Transform _sensorPosition;
    [SerializeField] float _sensorRadius;

    [SerializeField] LayerMask _groundLayer;

    Vector3 _playerGravity;
    
    void Awake()
    {
        _animator = GetComponent<Animator>();
        _controller = GetComponent<CharacterController>();

        _jumpAction = InputSystem.actions["Jump"];
        _moveAction = InputSystem.actions["Move"];

    }
    void Start()
    {
        
    }

    void Update()
    {
        _moveValue = _moveAction.ReadValue<Vector2>();

        if(_jumpAction.WasPressedThisFrame() && IsGrounded())
        {
            Jump();
        }

        Movement();
        Gravity();
    }

    void Movement()
    {
        Vector3 moveDirection = new Vector3(_moveValue.x, 0, _moveValue.y);
        _controller.Move(moveDirection * _movementSpeed * Time.deltaTime);
        _animator.SetFloat("X", _moveValue.x);
        _animator.SetFloat("Y", _moveValue.y);
    }

    bool IsGrounded() 
    {
        return Physics.CheckSphere(_sensorPosition.position, _sensorRadius, _groundLayer);
    }

    void Jump()
    {
        _playerGravity.y = Mathf.Sqrt(_jumpHight * _gravity * -2);
        _controller.Move(_playerGravity * Time.deltaTime);
        _animator.SetBool("IsJumping", true);
    }

    void Gravity()
    {
        if(!IsGrounded())
        {
            _playerGravity.y += _gravity * Time.deltaTime;
        }
        else if(IsGrounded() && _playerGravity.y < -1)
        {
            _playerGravity.y = -2;
            _animator.SetBool("IsJumping", false);
        }
        _controller.Move(_playerGravity * Time.deltaTime);
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(_sensorPosition.position,_sensorRadius);
    }
}



