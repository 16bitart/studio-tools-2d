using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlantNode : MonoBehaviour
{
    [SerializeField] private PlantData _data;

    public bool IsReadyForGathering => Time.time > _readyGrowTime;
    private bool _isFinishedGrowing;

    private float _startGrowTime;
    private float _readyGrowTime;
    private float _growTime => 60f * _data.GrowthRate;

    private SpriteRenderer _renderer;

    private void OnEnable()
    {
 
    }

    public void Initialize()
    {
        if (_renderer == null) _renderer = GetComponent<SpriteRenderer>();
        if (_renderer == null) _renderer = GetComponentInChildren<SpriteRenderer>();
        
        var randomGrowthChance = Random.value > .5f;
        if (randomGrowthChance)
        {
            StartGrow();
        }
        else
        {
            _isFinishedGrowing = true;
            ValidateSprite();
        }
    }

    void StartGrow()
    {
        _renderer.sprite = _data.GrowingSprite;
        _isFinishedGrowing = false;
        _startGrowTime = Time.time;
        _readyGrowTime = _startGrowTime + _growTime;
    }

    private void CheckGrowth()
    {
        if (!_isFinishedGrowing)
        {
            _isFinishedGrowing = IsReadyForGathering;
            ValidateSprite();
        }
    }

    private void ValidateSprite()
    {
        if (_isFinishedGrowing)
        {
            _renderer.sprite = _data.GrownSprite;
        }
        else
        {
            _renderer.sprite = _data.GrowingSprite;
        }
    }
    
    public GatherResult Gather()
    {
        var result = new GatherResult();
        result.Success = IsReadyForGathering;
        if (result.Success)
        {
            result.HungerValue = _data.HungerValue;
            result.TimeUntilReady = _growTime;
            StartGrow();
        }
        return result;
    }
    
    
}

public class GatherResult
{
    public bool Success { get; set; }
    public float HungerValue { get; set; }
    public float ThirstValue { get; set; }
    public float TimeUntilReady { get; set; }
}