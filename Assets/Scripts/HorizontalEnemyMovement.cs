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
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private Animator shroomAnimator;
    
    private bool _facingRight = true;
    private bool _isWalking = true;
    private bool _performAttack = false;
    private float _attackCountdown = 0f;
    private float _attackCooldown = 0f;
    private float _attackCooldownMax = 1f;
    
    private Vector2 _lookAt;

    private void Update()
    {
        CastRays();
        transform.rotation = _facingRight ? new Quaternion(0f, 0f, 0f, transform.rotation.w) :
                                            new Quaternion(0f, 180f, 0f, transform.rotation.w);

        if (_attackCountdown > 0f)
        {
            _attackCountdown -= Time.deltaTime;
            return;
        }

        if (_attackCooldown > 0)
        {
            _attackCooldown -= Time.deltaTime;
        }
        
        if (_performAttack)
        {
            shroomAnimator.Play("MoushroomAttack");
            _attackCountdown = 0.51f;
            _attackCooldown = _attackCooldownMax;
            _performAttack = false;
            return;
        }
        if (_isWalking)
        {
            shroomAnimator.Play("MoushroomWalk");
        }
    }

    private void FixedUpdate()
    {
        _lookAt = _facingRight ? new (0.6f, -0.4f) : new (-0.6f, -0.4f);
        
        if(_isWalking)
            transform.Translate(new Vector2(0.6f, -0.4f) * (speed * Time.deltaTime));
    }

    private void CastRays()
    {
        RaycastHit2D hit = CastRay(transform, _lookAt, floorLayer);
        if (hit.collider is null)
        {
            _facingRight = !_facingRight;
        }
        
        RaycastHit2D playerDetection = CastRay(transform, _facingRight ? Vector2.right : Vector2.left, playerLayer);
        if (playerDetection.collider is not null && _attackCooldown <= 0f)
        {
            _performAttack = true;
        }
    }
    
    private RaycastHit2D CastRay(Transform formTransform, Vector2 dir, LayerMask targetLayer)
    {
        RaycastHit2D hit = Physics2D.Raycast(formTransform.position, dir, rayLength, targetLayer);
        //Debug.DrawLine(formTransform.position, (Vector2)formTransform.position + (dir * rayLength), Color.yellow);
        Debug.DrawLine(formTransform.position, hit.point, Color.red);
        return hit;
    }

    public void Die()
    {
        shroomAnimator.Play("MoushroomDeath");
        Destroy(gameObject, 0.5f);
    }
}