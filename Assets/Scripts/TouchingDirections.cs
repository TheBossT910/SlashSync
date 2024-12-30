using UnityEngine;

public class TouchingDirections : MonoBehaviour
{
    public ContactFilter2D castFilter;
    public float groundDistance = 0.05f;
    public float wallDistance = 0.2f;
    public float ceilingDistance = 0.05f;

   CapsuleCollider2D touchingCol;
   Animator animator;
   
   RaycastHit2D[] groundHits = new RaycastHit2D[16];
   RaycastHit2D[] wallHits = new RaycastHit2D[16];
   RaycastHit2D[] ceilingHits = new RaycastHit2D[16];

    [SerializeField]
    private bool _isGrounded = true;
    public bool IsGrounded { get {
            return _isGrounded;
        } private set{
            _isGrounded = value;
            animator.SetBool(AnimationStrings.isGrounded, value);
        }
    }

    [SerializeField]
    private bool _isOnWall = true;
    public bool IsOnWall { get {
            return _isOnWall;
        } private set{
            _isOnWall = value;
            animator.SetBool(AnimationStrings.isOnWall, value);
        }
    }

    [SerializeField]
    private bool _isOnCeiling = true;
    //=> lets the code be updated. We check if the player is facing right based on the scale, if they are, we return right, if not, we return left
    private Vector2 wallCheckDirection => gameObject.transform.localScale.x > 0 ? Vector2.right : Vector2.left;

    public bool IsOnCeiling { get {
            return _isOnCeiling;
        } private set{
            _isOnCeiling = value;
            animator.SetBool(AnimationStrings.isOnCeiling, value);
        }
    }

    // Awake is called when the script instance is being loaded
    void Awake()
    {
        touchingCol = GetComponent<CapsuleCollider2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        IsGrounded = touchingCol.Cast(Vector2.down, castFilter, groundHits, groundDistance) > 0;
        IsOnWall = touchingCol.Cast(wallCheckDirection, castFilter, wallHits, wallDistance) > 0;
        IsOnCeiling = touchingCol.Cast(Vector2.up, castFilter, ceilingHits, ceilingDistance) > 0;
    }
}
