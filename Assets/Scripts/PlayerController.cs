using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using Unity.VisualScripting;
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
    private Vector2 _closestPossibleJumpDir = Vector2.positiveInfinity;
    
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
        
        _currArea = Area.GROUND;
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), groundCollider, true);
    }

    private void Update()
    {
        _input = _playerActions.FreeMovement.Movement.ReadValue<Vector2>();
        CastRays();
    }

    private void FixedUpdate()
    {
        PerformMovement();
    }

    private void SwitchArea(InputAction.CallbackContext context)
    {
        if (!_canJump) return;
        _transform.position += (Vector3)_closestPossibleJumpDir * jumpStrength;
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
       return Physics2D.Raycast(transform.position, dir, rayLength, _currArea == Area.GROUND ? tunnelLayer : groundLayer);
    }

    private void CheckRay(RaycastHit2D ray)
    {
        if (ray.collider is null) return;
        
        _canJump = true;
        // comapare if ray hit is closer than the actual closest hit point, if yes then overwrite 
        if (Vector2.Distance(transform.position, ray.point) <
            Vector2.Distance(transform.position, _closestPossibleJumpDir))
            _closestPossibleJumpDir = ((Vector3)ray.point - transform.position).normalized;
            
        Debug.DrawLine(transform.position, ray.point, Color.red);
    }
    
    private void CastRays()
    {
        _canJump = false;
        RaycastHit2D rightHit = CastRay(Vector2.right);
        CheckRay(rightHit);

        // Cast a ray to the left
        RaycastHit2D leftHit = CastRay(-Vector2.right);
        CheckRay(leftHit);

        // Cast a ray up
        RaycastHit2D upHit = CastRay(Vector2.up);
        CheckRay(upHit);

        // Cast a ray down
        RaycastHit2D downHit = CastRay(Vector2.down);
        CheckRay(downHit);
        
        //if nothing is hit
        if (rightHit.collider is null && leftHit.collider is null && upHit.collider is null && downHit.collider is null)
        {
            _canJump = false;
            _closestPossibleJumpDir = Vector2.positiveInfinity;
        }
    }
    
    private void PerformMovement()
    {
        _rb.velocity = new Vector2(_input.x * xSpeed, _input.y * ySpeed);
    }
}
