using System;
using UnityEngine;

public class HitboxCollider : MonoBehaviour
{
    public event Action<float> OnHit;

    public void Hit(float damage)
    {
        OnHit?.Invoke(damage);
    }
}