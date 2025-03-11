using Map.Pathfinding;
using UnityEngine;

namespace Map.Generator
{
    public struct MapRatio
    {
        public float wall;
        public float water;
        public float tree;
        public float grass;
        public float rock;
        public float empty;
    }

    public enum MapDetail
    {
        Wall = 0,
        Water = 1,
        Tree = 2,
        Grass = 3,
        Rock = 4,
        Empty = 5,
    }

    public class MapGenerator : MonoBehaviour
    {
        [Header("Ссылки")]
        [SerializeField] private Transform _map;

        [Header("Префабы")]
        [SerializeField] private GameObject _wallPrefab;
        [SerializeField] private GameObject _waterPrefab;
        [SerializeField] private GameObject _treePrefab;
        [SerializeField] private GameObject _grassPrefab;
        [SerializeField] private GameObject _rockPrefab;
        [SerializeField] private GameObject _emptyPrefab;

        [Header("Настройки")]
        [SerializeField] [Range(10, 400)] private int _xSize;
        [SerializeField] [Range(10, 400)] private int _ySize;
        [SerializeField] [Range(0.0001f, 0.9999f)] private float _seed;

        [Header("Расширенные настройки генерации")]
        [SerializeField] [Range(0, 1)] private float _wallRatio;
        [SerializeField] [Range(0, 1)] private float _waterRatio;
        [SerializeField] [Range(0, 100)] private float _treeRatio;
        [SerializeField] [Range(0, 100)] private float _grassRatio;
        [SerializeField] [Range(0, 100)] private float _rockRatio;
        [SerializeField] [Range(0, 100)] private float _emptyRatio;


        public void Awake()
        {
        }

        public void GenerateMap()
        {
            this.ClearMap(this._map);
            MapRatio ratio = this._GetRatio(
                this._wallRatio,
                this._waterRatio,
                this._treeRatio,
                this._grassRatio,
                this._rockRatio,
                this._emptyRatio
            );
            MapDetail[,] details = this._GenerateMapDetails(this._xSize, this._ySize, this._seed, ratio);
            this._GenerateMap(
                this._map,
                details,
                this._wallPrefab,
                this._waterPrefab,
                this._treePrefab,
                this._grassPrefab,
                this._rockPrefab,
                this._emptyPrefab
            );
        }

        private void ClearMap(Transform map)
        {
            for (int i = map.childCount - 1; i >= 0; i--)
            {
                DestroyImmediate(map.GetChild(i).gameObject);
            }
        }

        private MapRatio _GetRatio(float wall, float water, float tree, float grass, float rock, float empty)
        {
            MapRatio ratio = new MapRatio
            {
                wall = wall,
                water = water
            };

            float reserved = wall - water; // 0.8 - 0.2 = 0.6

            if (reserved > 0)
            {
                float sum = tree + grass + rock + empty; // 80
                float treeRatio = tree / sum; // 12 / 80 = ~0.15
                float grassRatio = grass / sum; // 27 / 80 = ~0.33
                float emptyRatio = empty / sum; // 40 / 80 = ~0.5
                float rockRatio = rock / sum; // 10 / 80 = ~0.125

                ratio.tree = treeRatio * reserved + water; // 0.15 * 0.6 + 0.2 = 0.29
                ratio.grass = grassRatio * reserved + ratio.tree; // 0.3 * 0.6 + 0.29 = 0.47
                ratio.empty = emptyRatio * reserved + ratio.grass; // 0.77
                ratio.rock = rockRatio * reserved + ratio.empty; // 0.842
            }

            return ratio;
        }

        private MapDetail[,] _GenerateMapDetails(int x, int z, float seed, MapRatio ratio)
        {
            MapDetail[,] details = new MapDetail[x, z];

            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < z; j++)
                {
                    float weight = Mathf.PerlinNoise(i * seed, j * seed);

                    if (weight > ratio.wall)
                    {
                        details[i, j] = MapDetail.Wall;
                    }
                    else if (weight < ratio.water)
                    {
                        details[i, j] = MapDetail.Water;
                    }
                    else
                    {
                        // Порядок важен
                        if (weight >= ratio.empty)
                        {
                            details[i, j] = MapDetail.Rock;
                        }
                        else if (weight >= ratio.grass)
                        {
                            details[i, j] = MapDetail.Empty;
                        }
                        else if (weight >= ratio.tree)
                        {
                            details[i, j] = MapDetail.Grass;
                        }
                        else
                            // else if (weight >= ratio.tree)
                        {
                            details[i, j] = MapDetail.Tree;
                        }
                    }
                }
            }

            return details;
        }

        private void _GenerateMap(
            Transform container,
            MapDetail[,] details,
            GameObject wallPrefab,
            GameObject waterPrefab,
            GameObject treePrefab,
            GameObject grassPrefab,
            GameObject rockPrefab,
            GameObject emptyPrefab
        )
        {
            for (int x = 0; x < details.GetLength(0); x++)
            {
                for (int z = 0; z < details.GetLength(1); z++)
                {
                    GameObject prefab;
                    switch (details[x, z])
                    {
                        case MapDetail.Wall:
                            prefab = wallPrefab;
                            break;
                        case MapDetail.Water:
                            prefab = waterPrefab;
                            break;
                        case MapDetail.Tree:
                            prefab = treePrefab;
                            break;
                        case MapDetail.Grass:
                            prefab = grassPrefab;
                            break;
                        case MapDetail.Rock:
                            prefab = rockPrefab;
                            break;
                        default:
                            prefab = emptyPrefab;
                            break;
                    }

                    GameObject mapItem = Instantiate(prefab, container);
                    mapItem.GetComponent<Transform>().localPosition = new Vector3(x, z, 0);
                }
            }
        }
    }
}