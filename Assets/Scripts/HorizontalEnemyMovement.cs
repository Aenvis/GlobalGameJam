using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalEnemyMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float rayLength;
    [SerializeField] private LayerMask floorLayer;
    
    private bool _facingRight = true;
    private Vector2 _lookAt;
    
    private void Update()
    {
        _lookAt = _facingRight ? new (1f, -0.4f) : new (-1f, -0.4f);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, _lookAt, rayLength, floorLayer);

        if (hit.collider is null)
        {
            _facingRight = !_facingRight;
        }    
        
        Debug.DrawLine(transform.position, hit.point, Color.yellow);
    }

    private void FixedUpdate()
    {
        transform.Translate((_facingRight ? Vector3.right : Vector3.left) * (speed * Time.deltaTime));
    }
}
