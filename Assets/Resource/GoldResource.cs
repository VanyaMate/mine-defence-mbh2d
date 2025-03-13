
using System;
using TMPro;
using UnityEngine;

namespace Resource
{
    [RequireComponent(typeof(BoxCollider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class GoldResource : MonoBehaviour
    {
        [SerializeField] private float _resourceAmount = 100f;
        [SerializeField] private TMP_Text _text;

        private float _currentResource;
        private bool _disabled = false;

        private void Awake()
        {
            this._currentResource = this._resourceAmount;
        }


        public float TakeResource(float amount)
        {
            if (this._disabled)
            {
                return 0;
            }

            if (this._currentResource - amount > 0)
            {
                this._currentResource -= amount;
                this._text.text = $"{this._currentResource}/{this._resourceAmount}";
                return amount;
            }
            else if (this._currentResource - amount == 0)
            {
                this._currentResource -= amount;
                this._disabled = true;
                this._text.text = "0";
                return amount;
            }
            else
            {
                float resourceAmount = this._currentResource;
                this._currentResource = 0;
                this._disabled = true;
                this._text.text = "0";
                return resourceAmount;
            }
        }
    }
}