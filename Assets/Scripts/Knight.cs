using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections))]
public class Knight : MonoBehaviour
{

    public float walkSpeed = 5f;
    public float walkStopRate = 0.05f;
    public DetectionZone attackZone;
    Rigidbody2D rb;
    Animator animator;
    TouchingDirections touchingDirections;
    public enum WalkableDirection { Right, Left }
    private WalkableDirection _walkDirection;
    //walk to the right by default
    private Vector2 walkDirectionVector = Vector2.right;

    public WalkableDirection WalkDirection {
        get { return _walkDirection; }
        set { 
            if (_walkDirection != value) {
                //flip the sprite
                gameObject.transform.localScale = new Vector2(gameObject.transform.localScale.x * -1, gameObject.transform.localScale.y);

                if (value == WalkableDirection.Right) {
                    walkDirectionVector = Vector2.right;
                } else if (value == WalkableDirection.Left) {
                    walkDirectionVector = Vector2.left;
            }
            _walkDirection = value; 
            }
        }
    }

    public bool _hasTarget = false;

    public bool HasTarget { 
        get 
        { 
            return _hasTarget; 
        } 
        private set 
        {
            _hasTarget = value;
            animator.SetBool(AnimationStrings.hasTarget, value);
        } 
    }

    public bool CanMove {
        get
        {
            return animator.GetBool(AnimationStrings.canMove);
        }
    }

    // Awake is called when the script instance is being loaded
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        touchingDirections = GetComponent<TouchingDirections>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //check if the attack zone has a target
        HasTarget = attackZone.detectedColliders.Count > 0;
    }

    void FixedUpdate()
    {   
        if(CanMove) {
            rb.linearVelocity = new Vector2(walkDirectionVector.x * walkSpeed, rb.linearVelocity.y);
        } else {
            rb.linearVelocity = new Vector2(Mathf.Lerp(rb.linearVelocity.x, 0, walkStopRate), rb.linearVelocity.y);
        }
        
        if (touchingDirections.IsGrounded && touchingDirections.IsOnWall) {
            FlipDirection();
        }
    }

    private void FlipDirection()
    {
        if (WalkDirection == WalkableDirection.Right) {
            WalkDirection = WalkableDirection.Left;
        } else if (WalkDirection == WalkableDirection.Left) {
            WalkDirection = WalkableDirection.Right;
        } else {
            Debug.LogError("Current walkable direction is not set to the legal values of right or left");
        }
    }
}
