using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.Burst.Intrinsics;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

internal enum Area
{
    GROUND = 0,
    TUNNEL
}

public class PlayerController : MonoBehaviour
{
    [Header("Movement physics")]
    [SerializeField] private float freeMovementSpeed;
    [SerializeField] private float strictMovementSpeed;
    [SerializeField] private float turnSpeed;
    [SerializeField] private float jumpStrength;
    [SerializeField] private List<MarkerManager> bodySegments = new List<MarkerManager>();
    
    [Header("Root form refs")]
    [SerializeField] private Rigidbody2D headRb;
    [SerializeField] private Collider2D headCollider;
    [SerializeField] private Transform headTransform;
    
    [Header("RootMan form refs")] 
    [SerializeField] private Rigidbody2D rootManRb;
    [SerializeField] private Collider2D rootManCollider;
    [SerializeField] private Transform rootManTransform;
    
    [Header("Map areas")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask tunnelLayer;
    [SerializeField] private Collider2D groundCollider;
    [SerializeField] private Collider2D tunnelCollider;
    
    [Header("Raycasting params")]
    [SerializeField] private float rayLength;

    [SerializeField] private CinemachineVirtualCamera mainCamera;
    
    
    
    //Raycasting and jumping 
    private bool _canJump = false;
    private Vector2 _facing;
    private Dictionary<Vector2, bool> _canJumpDir;

    
    //input system
    private PlayerActions _playerActions;
    private Vector2 _input;
    private bool _freeMovement = true;

    //delay between head and body
    private float _delay = .0f;
    
    private Area _currArea;
    
    private void Start()
    {
        _playerActions = new PlayerActions();
        _playerActions.FreeMovement.Enable();
        _playerActions.Jump.Enable();
        
        _playerActions.Jump.Jump.performed += SwitchArea;
        
        Physics2D.IgnoreCollision(headCollider, groundCollider, true);
        Physics2D.IgnoreCollision(rootManCollider, tunnelCollider, true);
        
        _canJumpDir = new Dictionary<Vector2, bool>()
        {
            { Vector2.up, false },
            { Vector2.down, false },
            { Vector2.left, false },
            { Vector2.right, false }
        };
        
        _currArea = Area.GROUND;
    }

    private void Update()
    {
        ReadInput();
        CastRays();

        if(_delay > 0f)_delay -= Time.deltaTime;
    }

    private void FixedUpdate()
    {
        PerformMovement();
    }

    private void ReadInput()
    {
        _input = _playerActions.FreeMovement.Movement.ReadValue<Vector2>();
        if (_freeMovement)
        {
            _facing = (headTransform.right);
            return;
        }
        if (Input.GetKeyDown(KeyCode.A)) _facing = Vector2.left;
        if (Input.GetKeyDown(KeyCode.W)) _facing = Vector2.up;
        if (Input.GetKeyDown(KeyCode.S)) _facing = Vector2.down;
        if (Input.GetKeyDown(KeyCode.D)) _facing = Vector2.right;
    }

    private void SwitchMovementMode()
    {
        _freeMovement = !_freeMovement;
    }

    private void SwitchArea(InputAction.CallbackContext context)
    {
        if (!_canJump)  return;
        if(_freeMovement)
            headTransform.position += (Vector3)_facing * jumpStrength;
        else if (!_freeMovement && _canJumpDir[_facing])
            rootManTransform.position += (Vector3)_facing * jumpStrength;
        else return;
        
        _currArea = _currArea == Area.GROUND ? Area.TUNNEL : Area.GROUND;

        switch (_currArea)
        {
        case Area.GROUND:
            headTransform.position = rootManTransform.position;
            rootManTransform.gameObject.SetActive(false);
            foreach (var segment in bodySegments)
            {
                segment.gameObject.SetActive(true);
            }
            Physics2D.IgnoreCollision(headCollider, groundCollider, true);
            mainCamera.LookAt = headTransform;
            mainCamera.Follow = headTransform;
            headTransform.right = _facing;
            if (_facing == Vector2.left) headTransform.Rotate(180.0f, 0.0f, 0.0f);
            break;
        case Area.TUNNEL:
            rootManTransform.position = headTransform.position;
            rootManTransform.gameObject.SetActive(true);
            foreach (var segment in bodySegments)
            {
                segment.gameObject.SetActive(false);
            }
            Physics2D.IgnoreCollision(rootManCollider, tunnelCollider, true);
            mainCamera.LookAt = rootManTransform;
            mainCamera.Follow = rootManTransform;
            break;
        default:
            throw new ArgumentOutOfRangeException();
        }
        SwitchMovementMode();
    }

    private RaycastHit2D CastRay(Transform formTransform, Vector2 dir)
    {
       RaycastHit2D hit = Physics2D.Raycast(formTransform.position, dir, rayLength, _freeMovement ? tunnelLayer : groundLayer);
       if (hit.collider is not null)
       {
           _canJump = true;
           Debug.DrawLine(formTransform.position, hit.point, Color.red);
       }
       
       if(!_freeMovement)
        _canJumpDir[dir] = hit.collider;
       
       return hit;
    }

    private void CastRays()
    {
        if (_freeMovement)
        {
            RaycastHit2D forwardHit = CastRay(headTransform, headTransform.right);
            if (forwardHit.collider is not null) return;
            _canJump = false;
        }
        else
        {
            RaycastHit2D rightHit = CastRay(rootManTransform ,Vector2.right);
            RaycastHit2D leftHit = CastRay(rootManTransform, Vector2.left);
            RaycastHit2D upHit = CastRay(rootManTransform, Vector2.up);
            RaycastHit2D downHit = CastRay(rootManTransform, Vector2.down);
            //if nothing is hit
            if (rightHit.collider is not null || leftHit.collider is not null || upHit.collider is not null ||
                downHit.collider is not null) return;
            _canJump = false;
        }
    }
    
    private void PerformMovement()
    {
        if (_freeMovement)
        {
            headRb.velocity = headTransform.right * (freeMovementSpeed * Time.fixedDeltaTime);
            headTransform.Rotate(new Vector3(0, 0, -turnSpeed * Time.fixedDeltaTime * _input.x));
        }
        else
        {
            rootManRb.velocity = new Vector2(_input.x * strictMovementSpeed, rootManRb.velocity.y);
        }

        if (_delay > 0.0f) return;
        if (!_freeMovement) return;
        for (var i = 1; i < bodySegments.Count; i++)
        {
            MarkerManager markerManager = bodySegments[i - 1];
            bodySegments[i].gameObject.transform.position = markerManager.GetMarkers()[0].Position;
            bodySegments[i].gameObject.transform.rotation = markerManager.GetMarkers()[0].Rotation;
            markerManager.GetMarkers().RemoveAt(0);
        }
    }
}
