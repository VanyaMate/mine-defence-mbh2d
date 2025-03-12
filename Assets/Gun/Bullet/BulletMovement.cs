using UnityEngine;

namespace Gun.Bullet
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class BulletMovement : MonoBehaviour
    {
        [SerializeField] private float _timeToLife = 2f;

        private float _currentTimeOfLife = 0f;

        private void Update()
        {
            this._currentTimeOfLife += Time.deltaTime;
            if (this._currentTimeOfLife > this._timeToLife)
            {
                Destroy(gameObject);
            }
        }
    }
}