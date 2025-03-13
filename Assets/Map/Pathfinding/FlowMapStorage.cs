using Friendly;
using Map.Generator;
using UnityEngine;

namespace Map.Pathfinding
{
    public class FlowMapStorage : MonoBehaviour
    {
        public static FlowMapStorage instance;

        [SerializeField] private float _regenerateTime = 1f;
        [SerializeField] private MapGenerator _mapGenerator;

        private float _regenerateTimer = 0f;
        private Vector2[,] _flowMap = new Vector2[,] { };

        private void Awake()
        {
            FlowMapStorage.instance = this;
        }

        private void Update()
        {
            if ((this._regenerateTimer += Time.deltaTime) > this._regenerateTime)
            {
                this._flowMap = MapFlowGenerator.GetFlowMap(
                    this._mapGenerator.GetCurrentMapDetails(),
                    FriendlyContainer.instance.GetPositionsArray()
                );

                this._regenerateTimer = 0f;
            }
        }

        public Vector2[,] GetCurrentFlowMap()
        {
            return this._flowMap;
        }

        public Vector2 GetVector2ByPosition(Vector2 position)
        {
            int x = Mathf.FloorToInt(position.x);
            int y = Mathf.FloorToInt(position.y);

            if (this._flowMap.GetLength(0) <= x || x < 0)
            {
                return Vector2.zero;
            }

            if (this._flowMap.GetLength(1) <= y || y < 0)
            {
                return Vector2.zero;
            }

            return this._flowMap[x, y];
        }
    }
}