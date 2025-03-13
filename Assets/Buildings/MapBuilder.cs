using Map.Generator;
using UI;
using UnityEngine;

namespace Buildings
{
    public class MapBuilder : MonoBehaviour
    {
        [Header("Подключения")] [SerializeField]
        private MapPointer _pointer;

        [SerializeField] private MapGenerator _mapGenerator;
        [SerializeField] private float _moveSpeed = 10f;

        private PlayerInput _playerInput;
        private Vector2Int _targetPosition = new Vector2Int();
        private bool _enabled = false;
        private Building _currentBuilding;

        private void Awake()
        {
            this._playerInput = new PlayerInput();
        }

        private void Update()
        {
            if (!this._enabled) return;

            if (this._playerInput.UI.Cancel.ReadValue<float>() > 0)
            {
                this.Disable();
                return;
            }

            Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPosition.z = 0;

            this._targetPosition = new Vector2Int(Mathf.RoundToInt(mouseWorldPosition.x),
                Mathf.RoundToInt(mouseWorldPosition.y));

            MapDetail detail = this._mapGenerator.GetMapDetailsByPosition(this._targetPosition);

            if (detail == MapDetail.Gold || detail == MapDetail.Water || detail == MapDetail.Wall)
            {
                this._pointer.SetPointerAccess(false);
                if (this._playerInput.UI.Click.ReadValue<float>() > 0)
                {
                    this.Disable();
                    return;
                }
            }
            else
            {
                if (this._playerInput.UI.Click.ReadValue<float>() > 0)
                {
                    if (this._currentBuilding)
                    {
                        Instantiate(this._currentBuilding, new Vector3(x: this._targetPosition.x, y: this._targetPosition.y), Quaternion.identity);
                    }
                    this.Disable();
                    return;
                }

                this._pointer.SetPointerAccess(true);
            }

            this._pointer.SetPointerPosition(Vector2.Lerp(this._pointer.transform.position, this._targetPosition,
                this._moveSpeed * Time.deltaTime));
        }

        public void Enable(Building building)
        {
            this._enabled = true;
            this._pointer.gameObject.SetActive(true);
            this._currentBuilding = building;
            this._pointer.SetRadius(building.GetRadius());
            this._playerInput.Enable();
        }

        public void Disable()
        {
            this._enabled = false;
            this._pointer.gameObject.SetActive(false);
            this._playerInput.Disable();
        }
    }
}