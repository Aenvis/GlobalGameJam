using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

internal enum Area
{
    GROUND = 0,
    TUNNEL
}

public class PlayerController : MonoBehaviour
{
    [Header("Movement physics")]
    [SerializeField] private float xSpeed;
    [SerializeField] private float ySpeed;
    [SerializeField] private float jumpStrength;
    
    
    [Header("Map areas")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask tunnelLayer;
    [SerializeField] private Collider2D groundCollider;
    [SerializeField] private Collider2D tunnelCollider;
    
    [Header("Raycasting params")]
    [SerializeField] private float rayLength;
    
    //Raycasting and jumping
    private bool _canJump = false;
    private Vector2 _facing;
    private Dictionary<Vector2, bool> _canJumpDir;

    private Rigidbody2D _rb;
    private Transform _transform;
    
    //input system
    private PlayerActions _playerActions;
    private Vector2 _input;
    
    private Area _currArea;
    
    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _transform = GetComponent<Transform>();
        _playerActions = new PlayerActions();
        _playerActions.FreeMovement.Enable();
        _playerActions.Jump.Enable();

        _playerActions.Jump.Jump.performed += SwitchArea;
        
        _canJumpDir = new Dictionary<Vector2, bool>()
        {
            { Vector2.up, false },
            { Vector2.down, false },
            { Vector2.left, false },
            { Vector2.right, false }
        };
        
        _currArea = Area.GROUND;
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), groundCollider, true);
    }

    private void Update()
    {
        ReadInput();
        CastRays();
    }

    private void FixedUpdate()
    {
        PerformMovement();
    }

    private void ReadInput()
    {
        _input = _playerActions.FreeMovement.Movement.ReadValue<Vector2>();
        if (Input.GetKeyDown(KeyCode.A)) _facing = Vector2.left;
        if (Input.GetKeyDown(KeyCode.W)) _facing = Vector2.up;
        if (Input.GetKeyDown(KeyCode.S)) _facing = Vector2.down;
        if (Input.GetKeyDown(KeyCode.D)) _facing = Vector2.right;
        
        Debug.Log($"Facing dir: {_facing}");
    }
    
    private void SwitchArea(InputAction.CallbackContext context)
    {
        if (!_canJump) return;
        if (!_canJumpDir[_facing]) return;
        _transform.position += (Vector3)_facing * jumpStrength;
        _currArea = _currArea == Area.GROUND ? Area.TUNNEL : Area.GROUND;

        switch (_currArea)
        {
        case Area.GROUND:  
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), groundCollider, true);
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), tunnelCollider, false);
            break;
        case Area.TUNNEL:
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), groundCollider, false);
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), tunnelCollider, true);
            break;
        default:
            throw new ArgumentOutOfRangeException();
        }
    }

    private RaycastHit2D CastRay(Vector2 dir)
    {
       RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, rayLength, _currArea == Area.GROUND ? tunnelLayer : groundLayer);
       if (hit.collider is not null)
       {
           _canJump = true;
           Debug.DrawLine(transform.position, hit.point, Color.red);
       }
       
       _canJumpDir[dir] = hit.collider;
       return hit;
    }

    private void CastRays()
    {
        RaycastHit2D rightHit = CastRay(Vector2.right);
        // Cast a ray to the left
        RaycastHit2D leftHit = CastRay(Vector2.left);
        // Cast a ray up
        RaycastHit2D upHit = CastRay(Vector2.up);
        // Cast a ray down
        RaycastHit2D downHit = CastRay(Vector2.down);

        //if nothing is hit
        if (rightHit.collider is not null || leftHit.collider is not null || upHit.collider is not null ||
            downHit.collider is not null) return;
        
        _canJump = false;
    }
    
    private void PerformMovement()
    {
        _rb.velocity = new Vector2(_input.x * xSpeed, _input.y * ySpeed);
    }
}
