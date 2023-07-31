using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
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
    private Transform relativeCamera;

    [SerializeField]
    private Transform avatar;

    [SerializeField]
    private float speed;

    private void OnEnable()
    {
        moveAction.action.Enable();
        runAction.action.Enable();
    }

    private void OnDisable()
    {
        moveAction.action.Disable();
        runAction.action.Disable();
    }

    private void Update()
    {
        if(runAction.action.IsPressed())
        {
            speed = 4;
        }
        else
        {
            speed = 2;
        }
        Vector3 cameraForward = relativeCamera.forward;
        Vector3 cameraRight = relativeCamera.right;
        cameraForward.y = 0;
        cameraRight.y = 0;
        cameraForward = cameraForward.normalized;
        cameraRight = cameraRight.normalized;
        inputMoveVector = moveAction.action.ReadValue<Vector2>();
        Vector3 moveDirection = (cameraForward * inputMoveVector.y + cameraRight * inputMoveVector.x);
        moveDirection = moveDirection.normalized;
        controller.SimpleMove(moveDirection * speed);
        animator.SetFloat("Magnitude", (moveDirection*speed).magnitude);

        if(moveDirection != Vector3.zero)
        {
            avatar.forward = moveDirection;
        }
    }
}
