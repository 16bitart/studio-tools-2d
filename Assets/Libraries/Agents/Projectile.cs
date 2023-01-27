using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
public class Projectile : MonoBehaviour
{
    [SerializeField] private CircleCollider2D _collider;
    [SerializeField] private Rigidbody2D _body;

    private Vector2 _launchPos;
    private Vector2 _hitPos;
    private bool _hit;
    private bool _inFlight;
    private bool _inactive = true;

    public bool DebugMode;


    private void OnEnable()
    {
        if (_collider == null)
        {
            _collider = GetComponent<CircleCollider2D>();
        }

        if (_body == null)
        {
            _body = GetComponent<Rigidbody2D>();
        }
    }

    public void Launch(Vector2 direction, float force)
    {
        _collider.enabled = true;
        _inFlight = true;
        _inactive = false;
        _launchPos = transform.position;
        _body.AddForce(direction * force, ForceMode2D.Impulse);
    }

    private void Update()
    {
        if (_hit && !_inactive)
        {
            _inFlight = false;
            _body.velocity = Vector2.MoveTowards(_body.velocity, Vector2.zero, _body.drag * Time.deltaTime);
        }

        if (_body.velocity.magnitude <= 0.1f)
        {
            _inFlight = false;
            _inactive = true;
            Destroy(gameObject, 10f);
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (_inFlight)
        {
            _hit = true;
            _inFlight = false;
            _hitPos = transform.position;
            _collider.enabled = false;

            var hitbox = col.gameObject.GetComponent<HitboxCollider>();
            if (hitbox != null)
            {
                hitbox.Hit(_body.velocity.magnitude);
            }
        }
    }

    private void OnDrawGizmos()
    {
        if(_inactive || !DebugMode) return;
        
        var radius = _collider.bounds.extents.magnitude;
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(_launchPos, radius);
        if (_hit)
        {
            Gizmos.DrawLine(_launchPos, _hitPos);
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(_hitPos, radius);
            Gizmos.color = Color.red;
            Gizmos.DrawLine(_hitPos, transform.position);
        }
        else
        {
            Gizmos.DrawLine(_launchPos, transform.position);
        }
    }
}