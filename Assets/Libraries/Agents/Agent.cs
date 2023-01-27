using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{
    [field: SerializeField] public Vector2 LookDirection { get; private set; }
    [field: SerializeField] public Vector2 Position { get; private set; }
}
