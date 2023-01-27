using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentInput : MonoBehaviour
{
    private Agent _agent;

    public event Action OnPrimaryInput;
    public event Action OnSecondaryInput;
    public event Action OnDashInput;
    public event Action OnInteractInput;
    public event Action OnItemSlot1;
    public event Action OnItemSlot2;
    public event Action OnQuickAction1;
    public event Action OnQuickAction2;
    public event Action OnActionBar1;
    public event Action OnActionBar2;
    public event Action OnActionBar3;
    public event Action OnActionBar4;
    public Vector2 MoveInput { get; protected set; }
    public Vector2 LookInput { get; protected set; }
    
    public bool RunInput { get; protected set; }

    private void Awake()
    {
        HookAgent();
    }

    private void OnEnable()
    {
        HookAgent();
    }
    
    private void HookAgent()
    {
        _agent = GetComponent<Agent>();
    }

    protected void InvokePrimary()
    {
        OnPrimaryInput?.Invoke();
    }

    protected void InvokeSecondary()
    {
        OnSecondaryInput?.Invoke();
    }

    protected void InvokeDash()
    {
        OnDashInput?.Invoke();
    }

    protected void InvokeInteract()
    {
        OnInteractInput?.Invoke();
    }

    protected void InvokeItemSlot1()
    {
        OnItemSlot1?.Invoke();
    }

    protected void InvokeItemSlot2()
    {
        OnItemSlot2?.Invoke();
    }

    protected void InvokeQuickAction1()
    {
        OnQuickAction1?.Invoke();
    }

    protected void InvokeQuickAction2()
    {
        OnQuickAction2?.Invoke();
    }

    protected void InvokeActionBar1()
    {
        OnActionBar1?.Invoke();
    }

    protected void InvokeActionBar2()
    {
        OnActionBar2?.Invoke();
    }

    protected void InvokeActionBar3()
    {
        OnActionBar3?.Invoke();
    }

    protected void InvokeActionBar4()
    {
        OnActionBar4?.Invoke();
    }

    protected virtual void SetMoveInput(Vector2 moveInput)
    {
        MoveInput = Vector2.ClampMagnitude(moveInput, 1f);
    }

    protected virtual void SetLookDirection(Vector2 lookInput)
    {
        LookInput = Vector2.ClampMagnitude(lookInput, 1f);
    }
}