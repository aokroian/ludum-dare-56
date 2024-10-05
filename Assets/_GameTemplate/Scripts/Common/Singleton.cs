using UnityEngine;

namespace _GameTemplate.Scripts.Common
{
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        public static T Instance { get; private set; }

        protected void Awake()
        {
            if (Instance == null)
            {
                Instance = this as T;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void DestroyInstance()
        {
            Destroy(gameObject);
            Instance = null;
        }
    }
}