using Unity.VisualScripting;
using UnityEngine;

namespace UI
{
    public class MapPointer : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _pointer;
        [SerializeField] private SpriteRenderer _radius;

        public void SetRadius(float radius)
        {
            this._radius.size = new Vector2(radius, radius);
        }

        public void SetPointerPosition(Vector2 position)
        {
            transform.position = position;
        }

        public void SetPointerAccess(bool access)
        {
            if (access)
            {
                this._pointer.color = new Color(0, 255, 0, .7f);
                this._radius.enabled = true;
            }
            else
            {
                this._pointer.color = new Color(255, 0, 0, .5f);
                this._radius.enabled = false;
            }
        }
    }
}