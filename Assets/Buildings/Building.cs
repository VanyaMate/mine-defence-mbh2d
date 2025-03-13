using UnityEngine;

namespace Buildings
{
    public class Building : MonoBehaviour
    {
        [SerializeField] private CircleCollider2D _radiusCollider;
        [SerializeField] private float _radius;

        private void Awake()
        {
            if (this._radiusCollider != null)
            {
                this._radiusCollider.radius = this._radius;
            }
        }

        public float GetRadius()
        {
            return this._radius;
        }
    }
}