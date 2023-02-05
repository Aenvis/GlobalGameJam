using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class HorizontalEnemyMovement : MonoBehaviour
{
    [SerializeField] private float speed = 8f;
    [SerializeField] private float rayLength = 5f;
    [SerializeField] private LayerMask floorLayer;
    [SerializeField] [CanBeNull] private Collider2D tunnelCollider;
    
    private bool _facingRight = true;
    private Vector2 _lookAt;
    
    private void Start()
    {
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), tunnelCollider);
    }

    private void Update()
    {
        CastRays();
    }

    private void FixedUpdate()
    {
        _lookAt = _facingRight ? new (0.6f, -0.4f) : new (-0.6f, -0.4f);
        
        transform.Translate(_lookAt * (speed * Time.deltaTime));
    }

    private void CastRays()
    {
        RaycastHit2D hit = CastRay(transform, _lookAt);
    }
    
    private RaycastHit2D CastRay(Transform formTransform, Vector2 dir)
    {
        RaycastHit2D hit = Physics2D.Raycast(formTransform.position, dir, rayLength, floorLayer);
        if (hit.collider is null)
        {
            _facingRight = !_facingRight;
        }
        Debug.DrawLine(formTransform.position, (Vector2)formTransform.position + (dir * rayLength), Color.yellow);
        Debug.DrawLine(formTransform.position, hit.point, Color.red);
        return hit;
    }
}