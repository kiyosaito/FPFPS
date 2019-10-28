using System.Collections.Generic;
using UnityEngine;

public sealed class InputManager : UnitySingleton<InputManager>
{
    #region Private Variables

    private int _mouseButtonIdx = 0;

    private Dictionary<InputKeys, KeyCode> _keyMapping = new Dictionary<InputKeys, KeyCode>();

    private float _mouseSensitivity = 1f;

    #endregion

    #region Public Properties

    public float MouseSensitivity
    {
        get { return _mouseSensitivity; }
        set { _mouseSensitivity = value; }
    }

    #endregion

    #region Private Functions

    protected override void Setup()
    {
        // Default keymapping
        _keyMapping.Add(InputKeys.MoveHorizontalPositive, KeyCode.D);
        _keyMapping.Add(InputKeys.MoveHorizontalNegative, KeyCode.A);
        _keyMapping.Add(InputKeys.MoveVerticalPositive, KeyCode.W);
        _keyMapping.Add(InputKeys.MoveVerticalNegative, KeyCode.S);
        _keyMapping.Add(InputKeys.Jump, KeyCode.Space);
        _keyMapping.Add(InputKeys.Escape, KeyCode.Escape);
    }

    #endregion

    #region Public Functions

    public enum AxisInputs
    {
        MoveHorizontal,
        MoveVertical,
        LookHorizontal,
        LookVertical,
    };

    public enum InputKeys
    {
        MoveHorizontalPositive,
        MoveHorizontalNegative,
        MoveVerticalPositive,
        MoveVerticalNegative,
        Jump,
        Escape,
    };

    public float GetAxis(AxisInputs axis)
    {
        float inputValue = 0f;

        switch (axis)
        {
            case AxisInputs.MoveHorizontal:
                inputValue += (Input.GetKey(_keyMapping[InputKeys.MoveHorizontalPositive]) ? 1f : 0f);
                inputValue += (Input.GetKey(_keyMapping[InputKeys.MoveHorizontalNegative]) ? -1f : 0f);
                break;
            case AxisInputs.MoveVertical:
                inputValue += (Input.GetKey(_keyMapping[InputKeys.MoveVerticalPositive]) ? 1f : 0f);
                inputValue += (Input.GetKey(_keyMapping[InputKeys.MoveVerticalNegative]) ? -1f : 0f);
                break;
            case AxisInputs.LookHorizontal:
                inputValue = Input.GetAxis("Mouse X") * _mouseSensitivity;
                break;
            case AxisInputs.LookVertical:
                inputValue = Input.GetAxis("Mouse Y") * _mouseSensitivity;
                break;
        }

        return inputValue;
    }

    public bool GetButton(InputKeys inputKey)
    {
        return Input.GetKey(_keyMapping[inputKey]);
    }

    public bool GetButtonDown(InputKeys inputKey)
    {
        return Input.GetKeyDown(_keyMapping[inputKey]);
    }

    public bool GetButtonUp(InputKeys inputKey)
    {
        return Input.GetKeyUp(_keyMapping[inputKey]);
    }

    public bool GetMouseButton()
    {
        return Input.GetMouseButton(_mouseButtonIdx);
    }

    public bool GetMouseButtonDown()
    {
        return Input.GetMouseButtonDown(_mouseButtonIdx);
    }

    public bool GetMouseButtonUp()
    {
        return Input.GetMouseButtonUp(_mouseButtonIdx);
    }

    #endregion
}
