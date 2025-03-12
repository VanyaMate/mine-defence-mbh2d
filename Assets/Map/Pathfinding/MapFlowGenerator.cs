using System.Collections.Generic;
using Map.Generator;
using UnityEngine;

namespace Map.Pathfinding
{
    public class MapFlowGenerator
    {
        private static readonly Dictionary<MapDetail, int> TerrainCost = new Dictionary<MapDetail, int>
        {
            { MapDetail.Wall, int.MaxValue },
            { MapDetail.Gold, int.MaxValue },
            { MapDetail.Water, 10 },
            { MapDetail.Tree, 5 },
            { MapDetail.Grass, 1 },
            { MapDetail.Sand, 2 },
            { MapDetail.Rock, 1 },
            { MapDetail.Empty, 1 }
        };

        public static Vector2[,] GetFlowMap(MapDetail[,] details, Vector2[] targetPositions)
        {
            int width = details.GetLength(0);
            int height = details.GetLength(1);
            Vector2[,] flowMap = new Vector2[width, height];
            int[,] distanceMap = new int[width, height];

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    distanceMap[x, y] = int.MaxValue;
                    flowMap[x, y] = Vector2.zero;
                }
            }

            Queue<Vector2Int> queue = new Queue<Vector2Int>();

            foreach (var target in targetPositions)
            {
                int targetX = Mathf.RoundToInt(target.x);
                int targetY = Mathf.RoundToInt(target.y);

                if (targetX >= 0 && targetX < width && targetY >= 0 && targetY < height)
                {
                    queue.Enqueue(new Vector2Int(targetX, targetY));
                    distanceMap[targetX, targetY] = 0;
                }
            }

            Vector2Int[] directions =
            {
                new Vector2Int(1, 0), new Vector2Int(-1, 0), new Vector2Int(0, 1), new Vector2Int(0, -1),
                new Vector2Int(1, 1), new Vector2Int(-1, -1), new Vector2Int(1, -1), new Vector2Int(-1, 1)
            };

            while (queue.Count > 0)
            {
                Vector2Int current = queue.Dequeue();
                int currentDistance = distanceMap[current.x, current.y];

                foreach (var dir in directions)
                {
                    Vector2Int neighbor = current + dir;

                    if (neighbor.x >= 0 && neighbor.x < width && neighbor.y >= 0 && neighbor.y < height)
                    {
                        int terrainCost = TerrainCost[details[neighbor.x, neighbor.y]];
                        if (terrainCost == int.MaxValue) continue;

                        int newDistance = currentDistance + terrainCost;
                        if (newDistance < distanceMap[neighbor.x, neighbor.y])
                        {
                            distanceMap[neighbor.x, neighbor.y] = newDistance;
                            queue.Enqueue(neighbor);
                        }
                    }
                }
            }

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (distanceMap[x, y] == int.MaxValue) continue;

                    int minDistance = distanceMap[x, y];
                    Vector2 bestDirection = Vector2.zero;

                    foreach (var dir in directions)
                    {
                        int nx = x + dir.x;
                        int ny = y + dir.y;

                        if (nx >= 0 && nx < width && ny >= 0 && ny < height)
                        {
                            if (distanceMap[nx, ny] < minDistance)
                            {
                                minDistance = distanceMap[nx, ny];
                                bestDirection = new Vector2(dir.x, dir.y).normalized;
                            }
                        }
                    }

                    flowMap[x, y] = bestDirection;
                }
            }

            return flowMap;
        }
    }
}