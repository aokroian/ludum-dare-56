using UnityEngine;
using Zenject;

namespace Level
{
    public class PropSocket : MonoBehaviour
    {
        [field: SerializeField] public BoxCollider Bounds { get; private set; }
        [field: SerializeField] public Vector2Int MinMaxSize { get; private set; }

        [Inject] private Level _level;

        private Prop _currentProp;

        private void Awake()
        {
        }

        public void SetProp(Prop prop)
        {
        }
    }
}