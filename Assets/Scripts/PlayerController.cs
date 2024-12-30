using System;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.InputSystem;

//make it so that it requires we have a Rigidbody2D on the GameObject
[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections))]
public class PlayerController : MonoBehaviour
{
    //the "f" is for float
    public float walkSpeed = 5f;
    public float runSpeed = 8f;
    public float airWalkSpeed = 3f;
    public float airRunSpeed = 6f;
    public float jumpImpulse = 10f;
    Vector2 moveInput;
    TouchingDirections touchingDirections;

    public float CurrentMoveSpeed {
        get {
            if(CanMove) {
                if (IsMoving && !touchingDirections.IsOnWall) {
                //ground speeds
                if (touchingDirections.IsGrounded) {
                    if (IsRunning) {
                        return runSpeed;
                    } else {
                        return walkSpeed;
                    }
                    //air speeds
                    } else {
                        if (IsRunning) {
                            return airRunSpeed;
                        } else {
                            return airWalkSpeed;
                        }
                    }
                
                } else {
                    //Idle speed is 0, player does not move when touching the wall or when movement keys are not pressed
                    return 0;
                }

            } else {
                //locked movement for combat animations
                return 0;
            }
        }
    }

    //use serialize field to see variable in Unity UI
    [SerializeField]
    private bool _IsMoving = false;
    public bool IsMoving { 
        get {
            return _IsMoving;  
        } private set {
            _IsMoving = value;
            animator.SetBool(AnimationStrings.isMoving, value);
        } 
    }

    [SerializeField]
    private bool _IsRunning = false;
    public bool IsRunning {
        get {
            return _IsRunning;
        } set {
            _IsRunning = value;
            animator.SetBool(AnimationStrings.isRunning, value);
        }
    }

    public bool _IsFacingRight = true;
    public bool IsFacingRight { 
        get {
            return _IsFacingRight;
        } private set {
            if (_IsFacingRight != value) {
                //flip the local scale to make the player (and all child objects) face the opposite direction
                transform.localScale *= new Vector2(-1, 1);

            }
            _IsFacingRight = value;
        } 
    }

    public bool CanMove {
        get {
            return animator.GetBool(AnimationStrings.canMove);
        }
    }

    Rigidbody2D rb;
    Animator animator;

    private void Awake() {
        //get the Rigidbody2D component
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        touchingDirections = GetComponent<TouchingDirections>();
    }

    private void FixedUpdate() {
        //moves the character. This immediately updates the Rigidbody2D's velocity
        //note how this depends on moveInput, which is in the OnMove method
        rb.linearVelocity = new Vector2(moveInput.x * CurrentMoveSpeed, rb.linearVelocity.y);
        animator.SetFloat(AnimationStrings.yVelocity, rb.linearVelocity.y);
    }

    //player inputs and logic
    public void OnMove(InputAction.CallbackContext context) {
        //retrives the current Vector2 to use in movement calculation
        moveInput = context.ReadValue<Vector2>();
        IsMoving = moveInput != Vector2.zero;
        SetFacingDirection(moveInput);
    }

    private void SetFacingDirection(Vector2 moveInput)
    {
        if (moveInput.x > 0 && !IsFacingRight) {
            //face the right
            IsFacingRight = true;
        } else if (moveInput.x <0 && IsFacingRight) {
            //face the left
            IsFacingRight = false;
        }
    }

    public void OnRun(InputAction.CallbackContext context) {
        //if the button is pressed
        if (context.started) {
            IsRunning = true;

            //if the button is let go
        } else if (context.canceled) {
            IsRunning = false;
        }
    }

    public void OnJump(InputAction.CallbackContext context) {
        //if the button is pressed
        if (context.started && touchingDirections.IsGrounded && CanMove) {
            animator.SetTrigger(AnimationStrings.jumpTrigger);
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpImpulse);
        }
    }

    public void OnAttack(InputAction.CallbackContext context) {
        //if the button is pressed
        if (context.started) {
            animator.SetTrigger(AnimationStrings.attackTrigger);
        }
    }
}
