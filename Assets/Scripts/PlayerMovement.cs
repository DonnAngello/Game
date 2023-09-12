using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
using UnityEngine.Windows;

public class PlayerMovement : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private CharacterController controller;

    public Animator animator;

    [SerializeField]
    private InputActionReference moveAction;
    private Vector2 inputMoveVector;
    [SerializeField]
    private InputActionReference runAction;
    [SerializeField]
    private InputActionReference jumpAction;
    [SerializeField]
    private InputActionReference kickAction;
    /*
    [SerializeField]
    private InputActionReference toggleConsoleAction;
    [SerializeField]
    private InputActionReference backAction;
    
    [SerializeField]
    private GameObject canvasChat;
    */
    //[SerializeField]
    private Transform relativeCamera;

    public Transform avatar;

    //[Header("Player")]
    [Tooltip("Move speed of the character in m/s")]
    public float MoveSpeed = 3.0f;

    // timeout deltatime
    private float _jumpTimeoutDelta;
    private float _fallTimeoutDelta;
    private float _verticalVelocity;
    private float _terminalVelocity = 53.0f;

    [Space(10)]
    [Tooltip("The height the player can jump")]
    public float JumpHeight = 1.2f;

    [Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
    public float Gravity = -15.0f;

    [Space(10)]
    [Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
    public float JumpTimeout = 0.50f;

    [Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
    public float FallTimeout = 0.15f;


    [Tooltip("Sprint speed of the character in m/s")]
    public float SprintSpeed = 5.0f;

    private float speed;
    public float jumpForce = 12f, gravityMod = 2.5f;

    public Transform groundCheckPoint;
    private bool isGrounded;
    public LayerMask groundLayers;

    void Start()
    {
        relativeCamera = Camera.main.transform;
        Debug.Log(relativeCamera.position);
    }
    public override void OnEnable()
    {
        moveAction.action.Enable();
        runAction.action.Enable();
        jumpAction.action.Enable();
        kickAction.action.Enable();
        //toggleConsoleAction.action.Enable();
        //backAction.action.Enable();
    }

    public override void OnDisable()
    {
        moveAction.action.Disable();
        runAction.action.Disable();
        jumpAction.action.Disable();
        kickAction.action.Disable();
        //toggleConsoleAction.action.Disable();
        //backAction.action.Disable();
    }

    private void Update()
    {
        if (photonView.IsMine)
        {
            if (runAction.action.IsPressed())
            {
                speed = SprintSpeed;
            }
            else
            {
                speed = MoveSpeed;
            }
            isGrounded = Physics.Raycast(groundCheckPoint.position, Vector3.down, .25f, groundLayers);
            Vector3 cameraForward = relativeCamera.forward;
            Vector3 cameraRight = relativeCamera.right;
            cameraForward.y = 0;
            cameraRight.y = 0;
            cameraForward = cameraForward.normalized;
            cameraRight = cameraRight.normalized;
            inputMoveVector = moveAction.action.ReadValue<Vector2>();
            Vector3 moveDirection = (cameraForward * inputMoveVector.y + cameraRight * inputMoveVector.x);
            moveDirection = (moveDirection.normalized * speed);
            Vector3 allAxesMove = (moveDirection * Time.deltaTime + new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);
            controller.Move(allAxesMove);

           
            photonView.RPC("Walk", RpcTarget.All, moveDirection.magnitude);


            JumpAndGravity();


            if (moveDirection != Vector3.zero)
            {
                photonView.RPC("SetMoveDirection", RpcTarget.All, moveDirection);
            }
            /*
            if (toggleConsoleAction.action.IsPressed())
            {
                moveAction.action.Disable();
                runAction.action.Disable();
                jumpAction.action.Disable();
                canvasChat.SetActive(true);
            }

            if (backAction.action.IsPressed())
            {
                moveAction.action.Enable();
                runAction.action.Enable();
                jumpAction.action.Enable();
                canvasChat.SetActive(false);
            }
            */

            if (kickAction.action.IsPressed())
            {
                photonView.RPC("AnimateKick", RpcTarget.All);
            }
        }
    }

    private void JumpAndGravity()
    {
        if (isGrounded)
        {
            // reset the fall timeout timer
            _fallTimeoutDelta = FallTimeout;

            photonView.RPC("AnimateJump", RpcTarget.All, false);

            // stop our velocity dropping infinitely when grounded
            if (_verticalVelocity < 0.0f)
            {
                _verticalVelocity = -2f;
            }

            // Jump
            if (jumpAction.action.IsPressed() && _jumpTimeoutDelta <= 0.0f)
            {
                // the square root of H * -2 * G = how much velocity needed to reach desired height
                _verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);

                //animator.SetBool("animJump", true);
                photonView.RPC("AnimateJump", RpcTarget.All, true);
            }

            // jump timeout
            if (_jumpTimeoutDelta >= 0.0f)
            {
                _jumpTimeoutDelta -= Time.deltaTime;
            }
        }
        else
        {
            // reset the jump timeout timer
            _jumpTimeoutDelta = JumpTimeout;

            // fall timeout
            if (_fallTimeoutDelta >= 0.0f)
            {
                _fallTimeoutDelta -= Time.deltaTime;
            }
        }

        // apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
        if (_verticalVelocity < _terminalVelocity)
        {
            _verticalVelocity += Gravity * Time.deltaTime;
        }
    }

    [PunRPC]
    void Walk(float walkSpeed)
    {
        if(animator != null)
        {
            animator.SetFloat("Magnitude", walkSpeed);
        }
        
    }

    [PunRPC]
    void SetMoveDirection(Vector3 direction)
    {
        if(animator != null)
        {
            avatar.forward = direction;
        }
    }

    [PunRPC]
    void AnimateJump(bool jump)
    {
        if(animator != null)
        {
            animator.SetBool("animJump", jump);
        }
    }

    [PunRPC]
    void AnimateKick()
    {
        if (animator != null)
        {
            animator.SetTrigger("animKick");
        }
    }
}
