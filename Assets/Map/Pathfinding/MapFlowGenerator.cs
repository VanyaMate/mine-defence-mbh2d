using System.Collections.Generic;
using UnityEngine;

// generated

namespace Map.Pathfinding
{
    public class FlowMapGenerator
    {
        private int _width;
        private int _height;
        private Vector2[,] _flowField;
        private int[,] _costField;
        private Queue<Vector2Int> _queue = new Queue<Vector2Int>();

        public FlowMapGenerator(int width, int height)
        {
            _width = width;
            _height = height;
            _flowField = new Vector2[width, height];
            _costField = new int[width, height];
        }

        public Vector2[,] GenerateFlowMap(Vector2Int[] targets, int[,] terrainCost)
        {
            _costField = terrainCost;
            _flowField = new Vector2[_width, _height];

            // Очистка карты направлений
            for (int x = 0; x < _width; x++)
            {
                for (int y = 0; y < _height; y++)
                {
                    _flowField[x, y] = Vector2.zero;
                }
            }

            // Добавляем все цели в очередь
            foreach (var target in targets)
            {
                _queue.Enqueue(target);
                _costField[target.x, target.y] = 0; // Цели имеют "нулевую стоимость"
            }

            // Запускаем волновой алгоритм
            while (_queue.Count > 0)
            {
                Vector2Int cell = _queue.Dequeue();
                int cellCost = _costField[cell.x, cell.y];

                foreach (Vector2Int neighbor in GetNeighbors(cell))
                {
                    if (_costField[neighbor.x, neighbor.y] > cellCost + 1)
                    {
                        _costField[neighbor.x, neighbor.y] = cellCost + 1;
                        _queue.Enqueue(neighbor);
                    }
                }
            }

            // Создаем векторное поле
            for (int x = 0; x < _width; x++)
            {
                for (int y = 0; y < _height; y++)
                {
                    Vector2 bestDirection = Vector2.zero;
                    int lowestCost = _costField[x, y];

                    foreach (Vector2Int neighbor in GetNeighbors(new Vector2Int(x, y)))
                    {
                        if (_costField[neighbor.x, neighbor.y] < lowestCost)
                        {
                            lowestCost = _costField[neighbor.x, neighbor.y];
                            bestDirection = ((Vector2)(neighbor - new Vector2Int(x, y))).normalized;
                        }
                    }

                    _flowField[x, y] = bestDirection;
                }
            }

            return _flowField;
        }

        private List<Vector2Int> GetNeighbors(Vector2Int cell)
        {
            List<Vector2Int> neighbors = new List<Vector2Int>();

            Vector2Int[] directions =
            {
                new Vector2Int(0, 1), new Vector2Int(0, -1),
                new Vector2Int(1, 0), new Vector2Int(-1, 0)
            };

            foreach (var dir in directions)
            {
                Vector2Int neighbor = cell + dir;
                if (neighbor.x >= 0 && neighbor.x < _width && neighbor.y >= 0 && neighbor.y < _height)
                {
                    neighbors.Add(neighbor);
                }
            }

            return neighbors;
        }
    }
}