using System;
using System.Collections.Generic;
using Resource;
using UnityEngine;

namespace Buildings
{
    [RequireComponent(typeof(CircleCollider2D))]
    public class MinerController : MonoBehaviour
    {
        [Header("Настройки")] [SerializeField] private float _mineSpeed = 1f;
        [SerializeField] private float _mineAmount = 1f;
        [SerializeField] private float _mineRadius = 2f;

        private float _mineTimer = 0f;
        private CircleCollider2D _circleCollider;
        private List<GoldResource> _resources = new List<GoldResource>();

        public void Awake()
        {
            this._circleCollider = this.GetComponent<CircleCollider2D>();
            this._circleCollider.radius = this._mineRadius;
            this._circleCollider.isTrigger = true;
        }

        public void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.TryGetComponent(out GoldResource goldResource))
            {
                this._resources.Add(goldResource);
            }
        }

        void FixedUpdate()
        {
            this._mineTimer += Time.fixedDeltaTime;

            if (this._mineTimer > this._mineSpeed && this._resources.Count > 0)
            {
                float amount = this._resources[0].TakeResource(this._mineAmount);
                if (Math.Abs(amount - this._mineAmount) > .001f)
                {
                    this._resources.Remove(this._resources[0]);
                }

                this._mineTimer = 0f;
            }
        }
    }
}