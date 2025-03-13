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
        public float gold;
        public float sand;
    }

    public enum MapDetail
    {
        Wall = 0,
        Water = 1,
        Tree = 2,
        Grass = 3,
        Rock = 4,
        Empty = 5,
        Gold = 6,
        Sand = 7,
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
        [SerializeField] private GameObject _goldPrefab;
        [SerializeField] private GameObject _sandPrefab;

        [Header("Настройки")]
        [SerializeField] [Range(10, 400)] private int _xSize;
        [SerializeField] [Range(10, 400)] private int _ySize;
        [SerializeField] [Range(0.1f, 0.2f)] private float _seedModificator;
        [SerializeField] [Range(1, 1000)] private int _xSeed;
        [SerializeField] [Range(1, 1000)] private int _ySeed;

        [Header("Расширенные настройки генерации")]
        [SerializeField] [Range(0, 1)] private float _wallRatio;
        [SerializeField] [Range(0, 1)] private float _waterRatio;
        [SerializeField] [Range(0, .1f)] private float _goldRatio;
        [SerializeField] [Range(0, 100)] private float _treeRatio;
        [SerializeField] [Range(0, 100)] private float _grassRatio;
        [SerializeField] [Range(0, 100)] private float _rockRatio;
        [SerializeField] [Range(0, 100)] private float _emptyRatio;
        [SerializeField] [Range(0, 100)] private float _sandRatio;


        private MapDetail[,] _currentMapDetails = new MapDetail[,] { };

        public void Awake()
        {
            this.GenerateMap();
        }


        public MapDetail[,] GenerateMap()
        {
            this._ClearMap(this._map);
            MapRatio ratio = this._GetRatio(
                this._wallRatio,
                this._waterRatio,
                this._treeRatio,
                this._grassRatio,
                this._rockRatio,
                this._emptyRatio,
                this._goldRatio,
                this._sandRatio
            );
            this._currentMapDetails = this._GenerateMapDetails(
                this._xSize,
                this._ySize,
                this._seedModificator,
                this._xSeed,
                this._ySeed,
                ratio
            );
            this._GenerateMap(
                this._map,
                this._currentMapDetails,
                this._wallPrefab,
                this._waterPrefab,
                this._treePrefab,
                this._grassPrefab,
                this._rockPrefab,
                this._emptyPrefab,
                this._goldPrefab,
                this._sandPrefab
            );

            return this._currentMapDetails;
        }

        public MapDetail[,] GetCurrentMapDetails()
        {
            return this._currentMapDetails;
        }

        public MapDetail GetMapDetailsByPosition(Vector2Int position)
        {
            if (position.x < 0 || position.x >= this._currentMapDetails.GetLength(0) || position.y < 0 ||
                position.y >= this._currentMapDetails.GetLength(1))
            {
                return MapDetail.Wall;
            }

            return this._currentMapDetails[position.x, position.y];
        }

        private void _ClearMap(Transform map)
        {
            for (int i = map.childCount - 1; i >= 0; i--)
            {
                DestroyImmediate(map.GetChild(i).gameObject);
            }
        }

        private MapRatio _GetRatio(
            float wall,
            float water,
            float tree,
            float grass,
            float rock,
            float empty,
            float gold,
            float sand
            )
        {
            MapRatio ratio = new MapRatio
            {
                wall = wall,
                water = water,
                gold = gold,
            };

            float reserved = wall - water - gold;

            if (reserved > 0)
            {
                float sum = tree + grass + rock + empty + sand;
                float treeRatio = tree / sum;
                float grassRatio = grass / sum;
                float emptyRatio = empty / sum;
                float rockRatio = rock / sum;
                float sandRatio = sand / sum;

                ratio.tree = treeRatio * reserved + water;
                ratio.grass = grassRatio * reserved + ratio.tree;
                ratio.empty = emptyRatio * reserved + ratio.grass;
                ratio.sand = sandRatio * reserved + ratio.empty;
                ratio.rock = rockRatio * reserved + ratio.sand;
            }

            return ratio;
        }

        private MapDetail[,] _GenerateMapDetails(int x, int z, float seed, int seedX, int seedY, MapRatio ratio)
        {
            MapDetail[,] details = new MapDetail[x, z];

            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < z; j++)
                {
                    float weight = Mathf.PerlinNoise((i + seedX) * seed, (j + seedY) * seed);

                    if (weight > ratio.wall)
                    {
                        details[i, j] = MapDetail.Wall;
                    }
                    else if (weight > (ratio.wall - ratio.gold))
                    {
                        details[i, j] = MapDetail.Gold;
                    }
                    else if (weight < ratio.water)
                    {
                        details[i, j] = MapDetail.Water;
                    }
                    else
                    {
                        // Порядок важен
                        if (weight >= ratio.sand)
                        {
                            details[i, j] = MapDetail.Rock;
                        }
                        else if (weight >= ratio.empty)
                        {
                            details[i, j] = MapDetail.Sand;
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
            GameObject emptyPrefab,
            GameObject goldPrefab,
            GameObject sandPrefab

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
                        case MapDetail.Gold:
                            prefab = goldPrefab;
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
                        case MapDetail.Sand:
                            prefab = sandPrefab;
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