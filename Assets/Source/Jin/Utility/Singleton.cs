using UnityEngine;

namespace Jin.Utility
{
    public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
    {
        protected static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType(typeof(T)) as T;

                    if (_instance != null)
                        _instance.Init();
                }

                if (_instance == null)
                {
                    var obj = new GameObject(string.Format("Singleton_{0}", typeof(T).Name));
                    _instance = obj.AddComponent(typeof(T)) as T;

                    _instance.Init();
                }

                return _instance;
            }
        }

        static private GameObject GetSingletonContainer()
        {
            GameObject SingletonContainer = GameObject.Find("SingletonContainer");
            if (SingletonContainer == null)
            {
                SingletonContainer = new GameObject("SingletonContainer");
                DontDestroyOnLoad(SingletonContainer);
            }

            return SingletonContainer;
        }

        private void Awake()
        {
            gameObject.transform.SetParent(GetSingletonContainer().transform);
        }

        private void OnDestroy()
        {
            _instance?.Release();
            _instance = null;
        }

        private void OnApplicationQuit()
        {
            _instance = null;
        }

        public virtual void Init() { }

        public virtual void Release() { }
    }
}