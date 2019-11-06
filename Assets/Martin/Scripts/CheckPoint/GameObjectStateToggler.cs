using UnityEngine;

public class GameObjectStateToggler : ResetableObject, Target
{
    #region Private Vaiables

    [SerializeField]
    private GameObject _initialOn = null;

    [SerializeField]
    private GameObject _initialOff = null;

    [SerializeField]
    private bool _toggled = false;

    private bool _savedToggled = false;

    #endregion

    #region Interface and abstract class implementations

    public void GetShot(bool charged, Vector3 point)
    {
        Toggle();
    }

    public override void ResetState()
    {
        _toggled = _savedToggled;
        SetGameObjectStates();
    }

    public override void SaveState()
    {
        _savedToggled = _toggled;
    }

    #endregion

    #region Public Functions

    public void TurnOn()
    {
        _toggled = false;
        SetGameObjectStates();
    }

    public void TurnOff()
    {
        _toggled = true;
        SetGameObjectStates();
    }

    public void Toggle()
    {
        _toggled = !_toggled;

        SetGameObjectStates();
    }

    #endregion

    #region Private Functions

    private void SetGameObjectStates()
    {
        if (null != _initialOn)
        {
            _initialOn.SetActive(!_toggled);
        }

        if (null != _initialOff)
        {
            _initialOff.SetActive(_toggled);
        }
    }

    #endregion
}
