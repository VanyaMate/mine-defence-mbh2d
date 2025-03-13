using System;
using UnityEngine;

namespace Friendly
{
    public class FriendlyTag : MonoBehaviour
    {
        public void Start()
        {
            FriendlyContainer.instance.Add(transform);
            gameObject.layer = LayerMask.NameToLayer("Friendly");
        }

        public void OnDestroy()
        {
            FriendlyContainer.instance.Remove(transform);
        }
    }
}