using UnityEngine;

namespace Internal
{
    public class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;

        public static T Instance {
            get {
                if (_instance == null) {
                    _instance = (T)FindObjectOfType (typeof(T));
 
                    if (_instance == null) {
                        _instance = new GameObject(typeof(T).ToString()).AddComponent<T>();
                    }
                }
 
                return _instance;
            }
        }

        protected bool CheckInstance ()
        {
            if (this == Instance) {
                return true;
            }
            Destroy (this);
            return false;
        }
    }
}
