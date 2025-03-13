using UI;
using UnityEngine;

namespace Unit
{
    public class HealthComponent : MonoBehaviour
    {
        [SerializeField] private HealthBar _bar;
        [SerializeField] private float _maxHealth = 100f;

        private float _currentHealth;

        private void Awake()
        {
            this._currentHealth = this._maxHealth;
        }

        public void TakeDamage(float damage)
        {
            this._currentHealth -= damage;

            if (this._currentHealth <= 0)
            {
                Destroy(gameObject);
            }

            this._bar.SetHealth(this._maxHealth, this._currentHealth);
        }

        public void Heal(float healAmount)
        {
            this._currentHealth += healAmount;

            if (this._currentHealth >= this._maxHealth)
            {
                this._currentHealth = this._maxHealth;
            }

            this._bar.SetHealth(this._maxHealth, this._currentHealth);
        }
    }
}