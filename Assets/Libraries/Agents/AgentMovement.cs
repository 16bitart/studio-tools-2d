using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentMovement : MonoBehaviour
{
    public Vector2 Velocity => _body.velocity;
    public float Speed => _body.velocity.magnitude;
    
    private Agent _agent;
    private Rigidbody2D _body;
    private Vector2 _targetVelocity;
    private Vector2 _dashVelocity;
    private float _momentum;

    private bool _runFlag;
    private bool _dashFlag;
    private int _dashFramesRemaining = 0;

    private bool _isFrozen = false;
    private float _freezeTimer = 0f;

    [SerializeField] private float _moveSpeed = 5.5f;
    [SerializeField] private float _runSpeed = 6.5f;
    [SerializeField] private float _accel = 20f;
    [SerializeField] private float _decel = 30f;
    [SerializeField] private float _dashForce = 4;
    [SerializeField] private int _dashFrameSkips = 4;
    private void Awake()
    {
        _agent = GetComponent<Agent>();
        _body = GetComponent<Rigidbody2D>();
    }
  
    public void FixedUpdate()
    {
        if (_isFrozen)
        {
            _freezeTimer -= Time.deltaTime;
            _isFrozen = _freezeTimer > 0f;
            if (_isFrozen)
            {
                if (Velocity.magnitude > 0f) _body.velocity = Vector2.zero;
                return;
            }
        }
        
        _momentum = _targetVelocity.magnitude > Speed ? _accel : _decel;
        HandleRun();
        HandleDash();
        if(!_dashFlag) SetVelocity();
    }
    
    private void HandleRun()
    {
        if(_dashFlag) return;
        if (_runFlag)
        {
            if (Velocity.magnitude < 0.0001f)
            {
                _runFlag = false;
                return;
            }
            
            //_agent.Stamina.Reduce(_agent.Configuration.BaseRunStaminaCost * Time.deltaTime);
            _runFlag = false;
        }
    }

    private void HandleDash()
    {
        if (_dashFlag)
        {
            if (_dashFramesRemaining == _dashFrameSkips)
            {
                /*var dashCost = _agent.Configuration.BaseDashStaminaCost;
                if (_agent.Stamina.Current > dashCost)
                {
                    _body.AddForce(_dashVelocity, ForceMode2D.Impulse);
                    _agent.Stamina.Reduce(dashCost);
                }*/
                _body.AddForce(_dashVelocity, ForceMode2D.Impulse);
            }
            
            if (_dashFramesRemaining == 0)
            {
                _dashFlag = false;
            }
            
            _dashFramesRemaining--;
        }
    }

    private void SetVelocity()
    {
        if(_targetVelocity.magnitude > 0.1f) _body.velocity = Vector2.MoveTowards(Velocity, _targetVelocity, _momentum * Time.deltaTime);
    }
    
    public void SetMoveInput(Vector2 moveInput, bool runInput)
    {
        _targetVelocity = moveInput * (runInput ? _moveSpeed : _runSpeed);
    }

    public void SetRunInput(bool runInput)
    {
        _runFlag = runInput;
    }

    public void TriggerDash()
    {
        if (!_dashFlag)
        {
            _dashVelocity = _targetVelocity.normalized * _dashForce;
            _dashFlag = true;
            _dashFramesRemaining = _dashFrameSkips;
        }
    }
    
    public void FreezeMovement(float time)
    {
        _isFrozen = true;
        _freezeTimer = time;
    }
}
