using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentCombat : MonoBehaviour
{
    private Agent _agent;
    private Vector2 _lastAttackPosition;
    private Vector2 _lastHitPoint;
    private float _lastHitRadius;
    private float _timeSinceLastHit;
    private bool _positiveHit;
    private Collider2D[] _hits = new Collider2D[100];
    public bool DebugEnabled { get; set; }

    private void Awake()
    {
        _agent = GetComponent<Agent>();
    }

    public void Melee(float damage, float range, float radius)
    {
        while (true)
        {
            Debug.Log("ATTACKING");
            var point = _agent.Position + (_agent.LookDirection * range);
            var size = Physics2D.OverlapCircleNonAlloc(point, radius, _hits);
            
            if (DebugEnabled)
            {
                _lastAttackPosition = _agent.Position;
                _lastHitPoint = point;
                _lastHitRadius = radius;
                _timeSinceLastHit = 0f;
                _positiveHit = size > 0;
            }

            for (int i = 0; i < _hits.Length; i++)
            {
                var hitGameObject = _hits[i]?.gameObject;
                if (hitGameObject != null)
                {
                    if(hitGameObject.transform.root.Equals(_agent.transform)) return;
                    hitGameObject.GetComponent<HitboxCollider>()?.Hit(damage);
                    if(DebugEnabled) Debug.Log($"HIT => -{damage}!");
                }
                _hits[i] = null;
            }

            break;
        }
    }

    /*
    public void Shoot(Projectile projectile, float force)
    {
        while (true)
        {
            projectile.Launch(_agent.LookDirection, force);
            Debug.Log("SHOOTING");
            break;
        }
    }*/

    public void Cast()
    {
        Debug.Log("CASTING");
    }

    public void DrawGizmos(float deltaTime)
    {
        if(!DebugEnabled) return;
        if (_timeSinceLastHit < 2f)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(_lastAttackPosition, .15f);
            Gizmos.color = Color.grey;
            Gizmos.DrawLine(_lastAttackPosition, _lastHitPoint);
            Gizmos.color = _positiveHit ? Color.green : Color.red;
            Gizmos.DrawWireSphere(_lastHitPoint, _lastHitRadius);
            _timeSinceLastHit += deltaTime;
        }
    }
}