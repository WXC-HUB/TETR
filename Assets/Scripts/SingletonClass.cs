using UnityEngine;

public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{
    public static T Instance = null;

    protected virtual void Awake()
    {
        Instance = this as T;
    }
}

public class Singleton<T> where T : class, new()
{
    private static T instance;
    private static readonly object lockObj = new object();

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                lock (lockObj)
                {
                    if (instance == null) instance = new T();
                }
            }
            return instance;
        }
    }
}
