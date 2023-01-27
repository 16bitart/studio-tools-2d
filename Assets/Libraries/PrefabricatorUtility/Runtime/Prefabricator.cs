using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace PrefabricatorUtility.Runtime
{
    [RequireComponent(typeof(Grid))]
    public class Prefabricator : MonoBehaviour
    {
        private static Prefabricator _instance;
        private Grid _grid;

        protected Grid grid
        {
            get
            {
                if (_grid == null)
                {
                    _grid = gameObject.AssertComponent<Grid>();
                }
                
                return _grid;
            }
        }

        public static Prefabricator Instance
        {
            get
            {
                if (_instance != null) return _instance;
                _instance = FindObjectOfType<Prefabricator>();
                if (_instance != null) return _instance;
            
            
                var go = new GameObject
                {
                    name = "Prefabricator"
                };
                    
                return _instance = go.AddComponent<Prefabricator>();
            }
        }

        [SerializeField] private List<PrefabricatorLayer> _layers = new();
        [SerializeField] private int _layerCount = 1;
        public int Layers => _layerCount;
        [SerializeField] private string _prefabPath;
        [SerializeField] private string _prefabName = "DefaultPrefabName";
        [SerializeField] private string _prefabFolderName = "Prefabrications";
        [SerializeField] private bool _clearTilemapOnPrefabCreation = true;

        [SerializeField] private TileBase _lastPaintedTile;
        [SerializeField] private Sprite _lastPaintedTileSprite;
        [SerializeField] private string _lastPaintedTileName;

        public void Awake()
        {
            BindInstance();
        }

        public void OnEnable()
        {
            BindInstance();
        }

        private void BindInstance()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        [ContextMenu("Reset All Layers")]
        public void ResetLayers()
        {
            if (_layers != null && _layers.Count == _layerCount)
            {
                foreach (var layer in _layers)
                {
                    layer.ClearLayer();
                }
            }
            else
            {
                ClearChildren();
                CreateAllLayers();
            }
        }

        [ContextMenu("Remove All Layers")]
        public void ClearChildren()
        {
            _layers ??= new List<PrefabricatorLayer>();
        
            if (_layers != null && _layers.Count > 0)
            {
                foreach (var layer in _layers.Where(l => l != null))
                {
                    DestroyImmediate(layer.gameObject);
                }
            }
        
            for (int i = 0; i < transform.childCount; i++)
            {
                var child = transform.GetChild(i);
                DestroyImmediate(child.gameObject);
            }
        
            _layers?.Clear();
        }

        [ContextMenu("Create Layers")]
        public void CreateAllLayers()
        {
            for (int i = 0; i < _layerCount; i++)
            {
                _layers.Add(CreateLayer(i));
            }
        }

        public void SetLastTilePainted(TileBase tile)
        {
            _lastPaintedTile = tile;
            _lastPaintedTileName = tile.name;
            _lastPaintedTileSprite = (tile as Tile)?.sprite;
        }
    
        public PrefabricatorLayer CreateLayer(int index)
        {
            var layerGameObj = new GameObject();
            layerGameObj.name = $"PrefabLayer{index}";
            var layer = layerGameObj.AddComponent<PrefabricatorLayer>();
            layer.Initialize(index);
            layer.transform.SetParent(transform);
            return layer;
        }

        public void CreatePrefabAsset(GameObject gameObj, string prefabName)
        {
            gameObj.name = prefabName;
            PrefabUtility.SaveAsPrefabAsset(gameObj, BuildAssetPath(gameObj));
            DestroyImmediate(gameObj);
        
            if (_clearTilemapOnPrefabCreation)
            {
                ClearChildren();
                CreateAllLayers();
            }
        }

        [ContextMenu("Create Prefab")]
        private void CreatePrefab()
        {
            var gameObj = new GameObject(_prefabName);
            var layerIndex = 1;
            var validLayers = _layers
                .Select(layer => layer.BakeTiles($"{_prefabName} [{layerIndex}]"))
                .Where(g => g != null);
        
            foreach (var layerObj in validLayers)
            {
                layerObj.transform.SetParent(gameObj.transform);
                layerIndex++;
            }

            gameObj.AddComponent<PrefabricationRootObject>();
            CreatePrefabAsset(gameObj, _prefabName);
        }
    
        private string BuildAssetPath(GameObject prefabGameObject)
        {
            if (string.IsNullOrWhiteSpace(_prefabPath) || !Directory.Exists(_prefabPath))
            {
                AssetDatabase.CreateFolder("Assets", _prefabFolderName);
                _prefabPath = Path.Combine("Assets", _prefabFolderName);
            }
            var prefabAssetName = prefabGameObject.name + ".prefab";
            return Path.Combine(_prefabPath, prefabAssetName);
        }
    }
}