using System;
using UnityEngine;

public abstract class BaseController : MonoBehaviour
{
    [SerializeField] private float yMoveSpeed;
    [SerializeField] private float xMoveSpeed;
    [SerializeField] private CharacterController controller;

    protected Vector2 p_MoveDir;

    private void Update()
    {
        p_MoveDir = GetMoveDir();
    }

    private void FixedUpdate()
    {
        MoveCharacter();
    }

    protected abstract Vector2 GetMoveDir();
    
    private void MoveCharacter()
    {
        controller.Move(new Vector2(p_MoveDir.x * xMoveSpeed, p_MoveDir.y * yMoveSpeed) * Time.fixedDeltaTime);
    }
}