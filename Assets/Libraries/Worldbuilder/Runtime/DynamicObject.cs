using System;
using PrefabricatorUtility.Runtime;
using UnityEngine;

public class DynamicObject : MonoBehaviour
{
    
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [field: SerializeField] public Vector2Int RenderSize { get; private set; }

    private void OnValidate()
    {
        _spriteRenderer = GetSpriteRenderer();
        var size = _spriteRenderer.sprite.bounds.size;
        RenderSize = new Vector2Int(Mathf.CeilToInt(size.x), Mathf.CeilToInt(size.y));
    }

    private SpriteRenderer GetSpriteRenderer()
    {
        if (_spriteRenderer == null)
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            if (_spriteRenderer == null)
            {
                _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            }
        }
        return _spriteRenderer;
    }
}