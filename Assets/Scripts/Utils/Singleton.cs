using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*! \brief Class implementing the Singleton pattern in Unity
 *
 *  This templatable class is an utility class.
 *  When extended, it implements the Singleton pattern to your Unity class.
 *  
 *  \tparam T Which class should become Singleton
 */
public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    private static T instance;

    /*! \brief Instanciation function.
     *
     *  \warning If you need to extend this function, use
     *  \warning the keyword new and call base.Awake() at the beginning
     *  \throws A FINIR
     */
    protected virtual void Awake()
    {
        if (instance != null)
            Debug.LogError("[Singleton] Trying to instantiate a second instance of a singleton class");
        else
            instance = (T)this;
    }

    protected virtual void OnDestroy()
    {
        instance = null;
    }

    public static T Instance
    {
        get { return instance; }
    }

    public bool IsInitialized
    {
        get
        {
            if (instance == null)
                return false;
            else return true;
        }
    }

}
