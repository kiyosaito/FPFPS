using UnityEngine;

public class GameObjectStateToggler : ResetableObject, Target
{
    [SerializeField]
    private GameObject _initialOn = null;

    [SerializeField]
    private GameObject _initialOff = null;

    [SerializeField]
    private bool _toggled = false;

    private bool _savedToggled = false;

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

    private void Toggle()
    {
        _toggled = !_toggled;

        SetGameObjectStates();
    }

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
}
