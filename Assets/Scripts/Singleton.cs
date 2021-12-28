using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Component
{
    private static T _instance;
    public static T Instance
    {
        get
        {
            if(_instance == null)
            {
                var objs = FindObjectsOfType(typeof(T)) as T[];
                if (objs.Length == 1)
                {
                    _instance = objs[0];

                } 
                else if (objs.Length > 1)
                {
                    Debug.LogError($"Multiple instances of {typeof(T).Name} found.");
                }
                else
                {
                    Debug.LogError($"No instances of {typeof(T).Name} found.");
                }
            }
            return _instance;
        }
    }
}
