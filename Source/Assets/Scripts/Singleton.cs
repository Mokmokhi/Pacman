using UnityEngine;

/*
 * Singleton class
 *
 * used for creating a singleton
 */

public class  Singleton<T> : MonoBehaviour where T : Component {
    private static T _instance;
    
    public static T Instance {
        // make sure there is only one instance
        get {
            if (_instance == null) {
                _instance = FindObjectOfType<T>();
                if (_instance == null) {
                    GameObject obj = new GameObject();
                    obj.name = typeof(T).Name;
                    _instance = obj.AddComponent<T>();
                }
            }
            return _instance;
        }
    }

    public virtual void Awake() {
        if (_instance == null) {
            _instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
        else {
            Destroy(gameObject);
        }
    }
}