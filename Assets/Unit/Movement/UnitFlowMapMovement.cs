using Map.Pathfinding;
using UnityEngine;

namespace Unit.Movement
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class UnitFlowMapMovement : MonoBehaviour
    {
        [SerializeField] private float _speed = 1f;

        private Rigidbody2D _rigidbody2D;
        private FlowMapStorage _flowMapStorage;

        private void Awake()
        {
            this._rigidbody2D = this.GetComponent<Rigidbody2D>();
        }

        private void Start()
        {
            this._flowMapStorage = FlowMapStorage.instance;
        }

        public void FixedUpdate()
        {
            Vector2 direction = this._flowMapStorage.GetVector2ByPosition(
                transform.position
            );

            this._rigidbody2D.MovePosition(
                this._rigidbody2D.position +
                direction * this._speed * Time.fixedDeltaTime
            );
        }
    }
}