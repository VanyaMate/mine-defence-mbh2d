using System;
using Unity.Cinemachine;
using UnityEngine;

namespace Player.InputMovement
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(CapsuleCollider2D))]
    public class PlayerInputMovement : MonoBehaviour
    {
        [Header("Настройки камеры")] [SerializeField]
        private CinemachineCamera _camera;

        [SerializeField] private float zoomSpeed = 2f;
        [SerializeField] private float minZoom = 2f;
        [SerializeField] private float maxZoom = 10f;

        [Header("Вращение головы")] [SerializeField]
        private Transform _head;

        private PlayerInput _playerInput;
        private Rigidbody2D _rigidbody2D;

        private void Awake()
        {
            this._playerInput = new PlayerInput();
            this._rigidbody2D = this.GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            Vector2 scrollDirection = _playerInput.UI.ScrollWheel.ReadValue<Vector2>();

            if (this._camera != null)
            {
                if (this._camera.Lens.Orthographic)
                {
                    float newSize = this._camera.Lens.OrthographicSize - scrollDirection.y * zoomSpeed * Time.deltaTime;
                    this._camera.Lens.OrthographicSize = Mathf.Clamp(newSize, minZoom, maxZoom);
                }
                else
                {
                    float newFOV = this._camera.Lens.FieldOfView - scrollDirection.y * zoomSpeed * Time.deltaTime;
                    this._camera.Lens.FieldOfView = Mathf.Clamp(newFOV, minZoom, maxZoom);
                }
            }

            Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 directionToMouse = (mouseWorldPosition - _head.position).normalized;
            float angle = Mathf.Atan2(directionToMouse.y, directionToMouse.x) * Mathf.Rad2Deg;
            _head.rotation = Quaternion.Euler(0, 0, angle);
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