using UnityEngine;

public class SingletonHandler<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (!_instance)
            {
                GameObject obj;
                obj = GameObject.Find(typeof(T).Name);
                if (!obj)
                {
                    obj = new GameObject(typeof(T).Name);
                    _instance = obj.AddComponent<T>();
                }
                else
                {
                    _instance = obj.GetComponent<T>();
                }
            }
            return _instance;
        }
    }

    public virtual void Awake()
    {
        GameObject obj = GameObject.Find(typeof(T).Name);

        if (!obj || !_instance)
        {
            _instance = gameObject.GetComponent<T>();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
