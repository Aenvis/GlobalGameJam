using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalEnemyMovement : MonoBehaviour
{
    [SerializeField] private float speed = 8;
    [SerializeField] private float rayLength = 5.0f;
    [SerializeField] private LayerMask floorLayer;
    private bool facingRight = true;
    
    

    private void FixedUpdate()
    {
        Vector2 direction = facingRight ? new (3f, -0.4f) : new (-3f, -0.4f);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, rayLength, floorLayer);

        if (hit.collider is null)
        {
            facingRight = !facingRight;
        }

        transform.Translate(direction * speed * Time.deltaTime);
        
        
        Debug.DrawLine(transform.position, hit.point, Color.yellow);
        
    }
}
