using UnityEngine;

namespace Map.Spawner
{
    public class MapSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject _spawnObject;
        [SerializeField] private float _spawnTime;

        private float _currentSpawnTime = 0f;

        void FixedUpdate()
        {
            this._currentSpawnTime += Time.fixedDeltaTime;

            if (this._currentSpawnTime >= this._spawnTime)
            {
                Instantiate(this._spawnObject, transform.position, Quaternion.identity);
                this._currentSpawnTime = 0;
            }
        }
    }
}