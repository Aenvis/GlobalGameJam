using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private CharacterController controller;
    [SerializeField] private float xSpeed;
    [SerializeField] private float ySpeed;

    private Vector2 _input;
    private PlayerActions _playerActions;
    void Start()
    {
        _playerActions = new PlayerActions();
        _playerActions.FreeMovement.Enable();
    }

    private void Update()
    {
        _input = _playerActions.FreeMovement.Movement.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        PerformMovement();
    }

    private void PerformMovement()
    {
        controller.Move(new Vector2(_input.x * xSpeed, _input.y * ySpeed) * Time.fixedDeltaTime);
    }
}
