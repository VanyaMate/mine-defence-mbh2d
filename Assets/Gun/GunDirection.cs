using System.Collections.Generic;
using Gun.Bullet;
using UnityEngine;

namespace Gun
{
    [RequireComponent(typeof(CircleCollider2D))]
    public class GunDirection : MonoBehaviour
    {
        [Header("Настройки")]
        [SerializeField] [Range(1, 15)] private float _radius = 1f;
        [SerializeField] [Range(1, 60)] private float _fireRate = 1f;

        [Header("Префабы")]
        [SerializeField] private BulletMovement _bulletPrefab;

        [Header("Внутренние настройки")]
        [SerializeField] private Transform _spawnPosition;
        [SerializeField] private int _enemyLayout = 6;

        private CircleCollider2D _collider;
        private List<Transform> _enemies;
        private float _fireTimer = 0f;
        private float _fireTimeMax;

        private void Awake()
        {
            this._fireTimeMax = 1 / this._fireRate;

            this._collider = this.GetComponent<CircleCollider2D>();
            this._collider.radius = this._radius;
            this._collider.isTrigger = true;
            this._enemies = new List<Transform>();
        }

        public void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.layer == this._enemyLayout)
            {
                this._enemies.Add(other.transform);
            }
        }

        public void OnTriggerExit2D(Collider2D other)
        {
            if (other.gameObject.layer == this._enemyLayout)
            {
                this._enemies.Remove(other.transform);
            }
        }

        void Update()
        {
            this._fireTimer += Time.deltaTime;

            if (this._enemies.Count > 0)
            {
                float nearestDistance = Mathf.Infinity;
                Transform nearestEnemy = null;

                this._enemies.ForEach((enemy) =>
                {
                    float distance = Vector2.Distance(enemy.position, transform.position);
                    if (nearestDistance > distance)
                    {
                        nearestEnemy = enemy;
                        nearestDistance = distance;
                    }
                });

                if (nearestEnemy != null)
                {
                    Vector2 direction = nearestEnemy.position - transform.position;
                    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                    transform.rotation = Quaternion.Euler(0, 0, angle);

                    if (this._fireTimer > this._fireTimeMax)
                    {
                        BulletMovement bullet = Instantiate(this._bulletPrefab, this._spawnPosition.position, Quaternion.identity);
                        bullet.GetComponent<Rigidbody2D>().AddForce(direction.normalized * 100);
                        this._fireTimer = 0f;
                    }
                }
            }
        }
    }
}