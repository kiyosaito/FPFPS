using System.Collections.Generic;
using UnityEngine;

// CRTP, MonoBehaviour based singleton
// Does not actually disable the creation of multiple instances, but extra instances will be destroyed
//   and getting the static instance reference will return a valid reference even if no instances existed before
public class UnitySingleton<T> : MonoBehaviour where T : UnitySingleton<T>
{
    #region Private Variables

    // Static lock to for managing thread safety
    private static readonly object instanceGetLock = new object();

    // Static instance reference
    private static T _instance = null;

    // Bool indicating is a particular instance was registered as the static reference
    private bool _registered = false;

    // Guard to make sure setup only runs once
    private bool _isSetup = false;

    #endregion

    #region Protected Functions

    // Virtual function determining if the component should persist over scene changes
    // True by default, override function if needed
    protected virtual bool ShouldSurviveSceneChange() { return true; }

    // Virtual function that will run exactly once before an Instance is accessed, override in derived class for setup
    protected virtual void Setup() { }

    protected bool IsRegistered { get { return _registered; } }

    #endregion

    #region Public Properties

    // Static instance property
    public static T Instance
    {
        get
        {
            // Lock for thread safety
            lock (instanceGetLock)
            {
                // Check if we already have a registered instance or not
                if (null == _instance)
                {
                    // If we do not have a registered instance, find all existing instances
                    // Other instances can exist, if they were part of the scene by default, but before they awoke, another object tried to access the singleton sooner
                    var existingInstances = new List<T>(FindObjectsOfType<T>());

                    // We destroy all excess instances, save for one
                    for (int i = 1; i < existingInstances.Count; ++i)
                    {
                        existingInstances[i].enabled = false;
                        Destroy(existingInstances[i]);
                    }

                    if (existingInstances.Count > 0)
                    {
                        // If we had at least one existing instance, then the first one will be used as the static reference
                        _instance = existingInstances[0];
                        _instance.Register();
                    }
                    else
                    {
                        // If there were no existing instances, we create a new one
                        _instance = new GameObject(typeof(T).Name + "Singleton").AddComponent<T>();
                        _instance.Register();
                    }
                }
            }

            return _instance;
        }
    }

    #endregion

    #region Private Functions

    // Register the instance as the static reference, and run the internal setup
    private void Register()
    {
        _registered = true;
        InternalSetup();
    }

    // Internal setup of the singleton
    private void InternalSetup()
    {
        // Check if we have set up before
        if (!_isSetup)
        {
            // Run the virtual setup function
            Setup();

            // Set the instance as don't destroy based on the settings
            if (ShouldSurviveSceneChange())
            {
                DontDestroyOnLoad(this);
            }

            // Remember that the setup has run
            _isSetup = true;
        }
    }

    #endregion

    #region MonoBehaviour Functions

    protected virtual void Start()
    {
        // Activate the lock for thread safety
        lock (instanceGetLock)
        {
            if ((!_registered) && (null == _instance))
            {
                // If this instance is not registered, and we don't have a static instance reference, make this the static reference and register it
                _instance = ((T)this);
                _instance.Register();
            }
            else if ((!_registered) && (null != _instance))
            {
                // If this instance is not registered, and we have a static instance reference, destroy this instance
                this.enabled = false;
                Destroy(this);
            }
        }
    }

    #endregion
}
