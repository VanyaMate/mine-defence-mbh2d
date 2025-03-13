using System;
using System.Collections.Generic;
using Unit;
using UnityEngine;

namespace Enemy
{
    [RequireComponent(typeof(CircleCollider2D))]
    public class EnemyAttack : MonoBehaviour
    {
        [Header("Настройки")]
        [SerializeField] private float _attackRadius = 1f;
        [SerializeField] private float _attackDamage = 10f;
        [SerializeField] private float _attackSpeed = .5f;
        [SerializeField] private List<HealthComponent> _friends = new List<HealthComponent>();

        private float _currentAttackTimer = 0f;

        private void Awake()
        {
            CircleCollider2D trigger = this.GetComponent<CircleCollider2D>();
            trigger.radius = this._attackRadius;
            trigger.isTrigger = true;
        }

        public void Update()
        {
            this._currentAttackTimer += Time.deltaTime;

            if (this._currentAttackTimer > this._attackSpeed && this._friends.Count > 0)
            {
                this._friends[0].TakeDamage(this._attackDamage);
                this._currentAttackTimer = 0f;
            }
        }

        public void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Friendly") &&
                other.TryGetComponent(out HealthComponent healthComponent))
            {
                this._friends.Add(healthComponent);
            }
        }

        public void OnTriggerExit2D(Collider2D other)
        {
            if (
                other.gameObject.layer == LayerMask.NameToLayer("Friendly") &&
                other.TryGetComponent(out HealthComponent healthComponent)
            )
            {
                this._friends.Remove(healthComponent);
            }
        }
    }
}