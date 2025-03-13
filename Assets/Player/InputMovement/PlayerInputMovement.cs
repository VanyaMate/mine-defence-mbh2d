using System;
using UnityEngine;

namespace Player.InputMovement
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(BoxCollider2D))]
    public class PlayerInputMovement : MonoBehaviour
    {
        private PlayerInput _playerInput;
        private Rigidbody2D _rigidbody2D;

        private void Awake()
        {
            this._playerInput = new PlayerInput();
            this._rigidbody2D = this.GetComponent<Rigidbody2D>();
        }

        void FixedUpdate()
        {
            this._rigidbody2D.MovePosition(
                this._rigidbody2D.position +
                this._playerInput.Player.Move.ReadValue<Vector2>() * Time.fixedDeltaTime * 5
            );
        }

        private void OnEnable()
        {
            this._playerInput.Enable();
        }

        private void OnDisable()
        {
            this._playerInput.Disable();
        }
    }
}