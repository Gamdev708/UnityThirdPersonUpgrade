using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour, Controls.IPlayerActions
{
    public bool IsAttacking { get; private set; }
    public bool IsBlocking { get; private set; }

    public Vector2 CameraTurnValue()
    {
        Vector2 mouseDelta = Input.mousePositionDelta;

        Vector2 value = new Vector2(-mouseDelta.y, mouseDelta.x);

        return value;
    }

    public Vector2 MovementValue { get; private set; }

    public bool IsHoldingJump;

    public event Action JumpEvent;
    public event Action DodgeEvent;
    public event Action TargetEvent;

    private Controls controls;

    private void Start()
    {
        controls = new Controls();
        controls.Player.SetCallbacks(this);

        controls.Player.Enable();
    }

    private void OnDestroy() 
    {
        controls.Player.Disable();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (!context.performed)
        {
            IsHoldingJump = false;
            return;
        }

        IsHoldingJump = true;

        if (JumpEvent != null)
        {
            JumpEvent.Invoke();
        }
    }

    public void OnDodge(InputAction.CallbackContext context)
    {
        if (!context.performed)
        {
            return;
        }

        if (DodgeEvent != null)
        {
            DodgeEvent.Invoke();
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        MovementValue = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        
    }

    public void OnTarget(InputAction.CallbackContext context)
    {
        if (!context.performed)
        {
            return;
        }

        if (TargetEvent != null)
        {
            TargetEvent.Invoke();
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            IsAttacking = true;
        }
        else if (context.canceled)
        {
            IsAttacking = false;
        }
    }

    public void OnBlock(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            IsBlocking = true;
        }
        else if (context.canceled)
        {
            IsBlocking = false;
        }
    }
}
