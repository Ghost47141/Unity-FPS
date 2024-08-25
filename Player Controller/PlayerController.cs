using System;
using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    #region Components
    Transform camera;
    InputAction moveAction;
    InputAction jumpAction;
    InputAction lookAction;
    InputAction crouchAction;
    [HideInInspector] public CharacterController charCtrl;
    [HideInInspector] public bool isMoving, isJumping, isLooking, isCrouching;
    public PlayerBaseState currentState;
    public IdleState idleState = new IdleState();
    public MoveState moveState = new MoveState();
    public JumpState jumpState = new JumpState();
    public CrouchState crouchState = new CrouchState();
    float cameraPitch = 0;
    [HideInInspector] public float velocityY = 0;
    Vector2 addedInertia = new Vector2(0,0);
    [HideInInspector] public bool inCrouchMode = false;

    #endregion

    #region Inspector Variables

    [SerializeField] float _mouseSensitivity = 0.2f;
    [SerializeField][Range(0f,.5f)] float _smoothTime = 0.025f;
    [Tooltip("Ctrl + P to stop play")][SerializeField] bool _isCursorHidden = false;
    public float _moveSpeed = 5f;
    public float _crouchSpeed = 2f;
    public float _jumpHeight = 3f;
    [Range(0f,1f)] public float _crouchTime = 0.25f;
    public float _gravity = -9.81f;
    public Vector3 _standingCenter = new Vector3(0,1f,0);
    public Vector3 _crouchingCenter = new Vector3(0,1.5f,0);
    public float _standingHeight = 2f;
    public float _crouchingHeight = 1f;
    
    #endregion

//---------------------CODE-------------------------

    void Awake() {
        charCtrl = GetComponent<CharacterController>();
        camera = Camera.main.transform;
    }
    void Start()
    {
        currentState = idleState;
        currentState.Init(this);

        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
        lookAction = InputSystem.actions.FindAction("Look");
        crouchAction = InputSystem.actions.FindAction("Crouch");
        
        if(_isCursorHidden) {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    void Update()
    {
        currentState.Iterate(this);
        HandleMouseLook();
    }
    void LateUpdate() {
        isMoving = moveAction.ReadValue<Vector2>().x > 0 || moveAction.ReadValue<Vector2>().y > 0;
        //isJumping = jumpAction.ReadValue<float>() > 0;
        isJumping = jumpAction.triggered;
        isLooking = lookAction.ReadValue<Vector2>().x > 0 || lookAction.ReadValue<Vector2>().y > 0;
        //isCrouching = crouchAction.ReadValue<float>() > 0;
        isCrouching = crouchAction.triggered;
    }
    void HandleMouseLook() {
        Vector2 mouseDelta = lookAction.ReadValue<Vector2>();
        transform.Rotate(Vector3.up, mouseDelta.x * _mouseSensitivity);
        {
            cameraPitch -= mouseDelta.y * _mouseSensitivity;
            cameraPitch = Mathf.Clamp(cameraPitch, -90, 90);
            camera.localEulerAngles = Vector3.right * cameraPitch;
        }
    }
    public Vector3 ProcessMovement(float moveSpeed) {
        Vector2 currentDirVelocity = new Vector2(0,0);
        Vector2 WASD = moveAction.ReadValue<Vector2>();
        addedInertia = Vector2.SmoothDamp(addedInertia, WASD, ref currentDirVelocity, _smoothTime);
        Vector3 moveForce = (transform.forward * addedInertia.y + transform.right * addedInertia.x) * moveSpeed;
        return moveForce * Time.deltaTime;
    }
    public Vector3 ProcessGravity() {
        
        if(charCtrl.isGrounded && velocityY < 0)
        {
            velocityY = -0.01f;
            return Vector3.up * velocityY;
        }
        else 
        {
            velocityY = Mathf.Clamp(velocityY, _gravity, Mathf.Infinity);
            velocityY += _gravity * Time.deltaTime;
            return Vector3.up * velocityY * Time.deltaTime;
        }
    }
    public void SwitchState(PlayerBaseState newState) {
        currentState = newState;
        currentState.Init(this);
    }
}