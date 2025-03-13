using System;
using Buildings;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIManager : MonoBehaviour
    {
        [Header("Подключение")]
        [SerializeField] private MapBuilder _builder;

        [Header("Кнопки")]
        [SerializeField] private Button _minerButton;
        [SerializeField] private Button _towerButton;
        [SerializeField] private Button _barricadeButton;

        [Header("Постройки")]
        [SerializeField] private Building _minerBuilding;
        [SerializeField] private Building _towerBuilding;
        [SerializeField] private Building _barricadeBuilding;

        public void Awake()
        {
            this._minerButton.onClick.AddListener(() => { this._builder.Enable(this._minerBuilding); });
            this._towerButton.onClick.AddListener(() => { this._builder.Enable(this._towerBuilding); });
            this._barricadeButton.onClick.AddListener(() => { this._builder.Enable(this._barricadeBuilding); });
        }
    }
}