using UnityEngine;

public class InputManager
{
    #region Singleton

    private readonly object instanceGetLock = new object();

    private static InputManager _instance = null;

    public InputManager Instance
    {
        get
        {
            lock (instanceGetLock)
            {
                if (null == _instance)
                {
                    _instance = new InputManager();
                }
            }

            return _instance;
        }
    }

    private InputManager() { }

    #endregion
}
