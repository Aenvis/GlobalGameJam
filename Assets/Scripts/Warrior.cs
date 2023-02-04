using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warrior : MonoBehaviour
{
    [SerializeField] private float speed = 8;
    [SerializeField] private float rayLength = 3.0f;
    [SerializeField] private LayerMask floorLayer;
    private bool facingRight = true;
    
    

    private void FixedUpdate()
    {
        /*
        Vector2 dir = new Vector2(1.0f, 0.0f);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, rayLength, floorLayer);
        */
        
        /*
        Vector2 direction = facingRight ? Vector2.right : Vector2.left;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, rayLength, _currArea == Area.GROUND ? tunnelLayer : groundLayer);

        if (hit.collider == null)
        {
            facingRight = !facingRight;
        }
        else
        {
            transform.Translate(direction * speed * Time.deltaTime);
        }
        */
        
        
        
        Vector2 direction = facingRight ? Vector2.right : Vector2.left;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 1.0f, 1 << LayerMask.NameToLayer("Wall"));

        if (hit.collider != null)
        {
            facingRight = !facingRight;
        }

        transform.Translate(direction * speed * Time.deltaTime);
        
        
        Debug.DrawLine(transform.position, hit.point, Color.yellow);
        
    }
}
