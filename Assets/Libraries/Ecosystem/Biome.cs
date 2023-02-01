using System.Collections.Generic;
using Libraries.Ecosystem;
using UnityEngine;


public class Biome : MonoBehaviour
{
    [SerializeField] private BiomeData _data;
    [SerializeField] private List<GameObject> _trees;
    [SerializeField] private List<GameObject> _plants;
}