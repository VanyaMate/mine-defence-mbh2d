using UnityEngine;

// generated

namespace Map.Pathfinding
{
    public class FlowMapVisualizer : MonoBehaviour
    {
        public Vector2[,] flowMap;
        public int width;
        public int height;
        public float scale = 0.5f; // Длина стрелки

        private void OnDrawGizmos()
        {
            if (flowMap == null) return;

            Gizmos.color = Color.red;

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Vector2 dir = flowMap[x, y];
                    if (dir != Vector2.zero)
                    {
                        Vector3 start = new Vector3(x, y, 0);
                        Vector3 end = start + new Vector3(dir.x, dir.y, 0) * scale;

                        Gizmos.DrawLine(start, end);
                        DrawArrowHead(end, dir);
                    }
                }
            }
        }

        private void DrawArrowHead(Vector3 position, Vector2 direction)
        {
            float arrowSize = 0.2f;
            Vector3 right = Quaternion.Euler(0, 0, 30) * -direction * arrowSize;
            Vector3 left = Quaternion.Euler(0, 0, -30) * -direction * arrowSize;

            Gizmos.DrawLine(position, position + right);
            Gizmos.DrawLine(position, position + left);
        }
    }
}