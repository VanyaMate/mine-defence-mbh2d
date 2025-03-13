using UnityEngine;

namespace UI
{
    public class HealthBar : MonoBehaviour
    {
        [Header("Настройки")]
        [SerializeField] private float _fullWidth;
        [SerializeField] private Transform _healthBar;


        public void SetHealth(float max, float current)
        {
            float width = this._fullWidth / max * current;
            this._healthBar.localScale = new Vector2(width, this._healthBar.localScale.y);
            this._healthBar.localPosition = new Vector3((width - this._fullWidth) / 2, 0, 0);
        }
    }
}