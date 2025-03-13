using System;
using Unit;
using UnityEngine;

namespace Gun.Bullet
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(CircleCollider2D))]
    public class BulletMovement : MonoBehaviour
    {
        [SerializeField] private float _timeToLife = 2f;
        [SerializeField] private float _damage = 35f;

        private float _currentTimeOfLife = 0f;

        private void Awake()
        {
            gameObject.layer = LayerMask.NameToLayer("Bullet");
        }

        private void Update()
        {
            this._currentTimeOfLife += Time.deltaTime;
            if (this._currentTimeOfLife > this._timeToLife)
            {
                Destroy(gameObject);
            }
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.layer != LayerMask.NameToLayer("Friendly"))
            {
                if (other.gameObject.TryGetComponent(out HealthComponent healthComponent))
                {
                    healthComponent.TakeDamage(this._damage);
                }

                Destroy(gameObject);
            }
        }
    }
}