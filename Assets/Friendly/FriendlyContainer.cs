using System.Collections.Generic;
using UnityEngine;

namespace Friendly
{
    public class FriendlyContainer : MonoBehaviour
    {
        public static FriendlyContainer instance;

        private List<Transform> _friendlyList = new List<Transform>();

        public void Awake()
        {
            FriendlyContainer.instance = this;
        }

        public void Add(Transform transform)
        {
            this._friendlyList.Add(transform);
        }

        public void Remove(Transform transform)
        {
            this._friendlyList.Remove(transform);
        }

        public List<Transform> GetCurrentList()
        {
            return this._friendlyList;
        }

        public Vector2[] GetPositionsArray()
        {
            Vector2[] positions = new Vector2[this._friendlyList.Count];

            for (int i = 0; i < this._friendlyList.Count; i++)
            {
                Vector3 position = this._friendlyList[i].position;
                positions[i] = new Vector2() { x = position.x, y = position.y };
            }

            return positions;
        }
    }
}